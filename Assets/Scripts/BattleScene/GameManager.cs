using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Cyberpunk;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BattleScene
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerDeckManager player1DeckManager;
        [SerializeField] private PlayerDeckManager player2DeckManager;
        [SerializeField] private PlayerHandManager player1HandManager;
        [SerializeField] private PlayerHandManager player2HandManager;
        [SerializeField] private EnemyDeckManager enemyDeckManager;
        [SerializeField] private EnemyHandManager enemyHandManager;
        [SerializeField] private DiscardManager player1DiscardManager;
        [SerializeField] private DiscardManager player2DiscardManager;
        [SerializeField] private DiscardManager enemyDiscardManager;
        [SerializeField] private BattleLog battleLog;
        [SerializeField] private GameObject heroPrefab;
        [SerializeField] private GameObject tacticCardPrefab;
        [SerializeField] private GameObject battleField;
        [SerializeField] private GameObject marker;
        [SerializeField] private TacticCard bioCommonTactic;
        [SerializeField] private TacticCard cyberCommonTactic;
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Canvas intermediateCanvas;
        [SerializeField] private Canvas endGameCanvas;
        [SerializeField] private TextMeshProUGUI log;
        [SerializeField] private TextMeshProUGUI finalText;
        [SerializeField] private TextMeshProUGUI playedCardCount;
        [SerializeField] private TMP_Text player1Balance;
        [SerializeField] private TMP_Text player2Balance;
        [SerializeField] private TMP_Text endPhaseButtonText;
        
        private Hero _player1Hero;
        private Hero _player2Hero;
        private bool _isWaitingForTarget = false, _cardsShowed = false, _spellsShowed = false, _giveUpButtonPressed = false;
        private int _turnNumber = 0;
        private int _turnPhase = 0;
        private int _initializedCardsCount = 0;
        private BattleField _battleField;
        private FieldSector[] _player1Sectors, _player2Sectors;
        private Card _lastCardClicked;
        private GameObject _player1, _player2;
        private GameObject[] _player1UI, _player2UI, _sectors;
        private bool _isUpdated, _isMarked;

        private void Awake()
        {
            endTurnButton.onClick.AddListener(() => _turnPhase += 1);
            giveUpButton.onClick.AddListener(() => _giveUpButtonPressed = true);
        }

        private void Start()
        {
            _player1Hero = IntersceneData.Instance.Player1;
            _player2Hero = IntersceneData.Instance.Player2;
            _battleField = FindAnyObjectByType<BattleField>();
            _player1Sectors = _battleField.transform.GetChild(0).GetComponentsInChildren<FieldSector>();
            _player2Sectors = _battleField.transform.GetChild(1).GetComponentsInChildren<FieldSector>();
            
            _player1UI = GameObject.FindGameObjectsWithTag("Player1UI");
            _player2UI = GameObject.FindGameObjectsWithTag("Player2UI");
            _sectors = GameObject.FindGameObjectsWithTag("BattleSector");
            
            InitPlayer1();
        }

        private void InitPlayer1()
        {
            _player1 = Instantiate(heroPrefab, _player1Sectors[4].transform.GetChild(1));
            _player1.transform.localScale = new Vector3(0.85f, 0.9f, 0.9f);
            _player1.AddComponent(_player1Hero.GetType());
            _player1.GetComponent<Hero>().balanceText = player1Balance;
            _player1.GetComponent<Card>().Sector = 4;
            UpdateVulnerability(_player1.GetComponent<Card>());
            _player1.GetComponent<Card>().OnCardClicked += CardClickHandle;
            _player1.GetComponent<Hero>().OnCardStateChanged += battleLog.AddLogMessage;

            var tactic1 = Instantiate(tacticCardPrefab, _player1Sectors[4].transform.GetChild(0));
            switch (_player1Hero.Tech)
            {
                case "Cyber":
                    tactic1.AddComponent(cyberCommonTactic.GetType());
                    break;
                case "Bio":
                    tactic1.AddComponent(bioCommonTactic.GetType());
                    break;
            }

            tactic1.GetComponent<TacticCard>().Owner = _player1.GetComponent<Hero>();
            tactic1.GetComponent<TacticCard>().Initialize();
            tactic1.GetComponent<TacticCard>().Sector = 4;
            tactic1.GetComponent<TacticCard>().OnDisabled -= () => TransferToDiscard(tactic1);
            tactic1.GetComponent<TacticCard>().OnDisabled += () => TransferToDiscard(tactic1);
            tactic1.GetComponent<TacticCard>().OnInitialized -= () => DeleteCardFromHand(tactic1);
            tactic1.GetComponent<TacticCard>().OnInitialized += () => DeleteCardFromHand(tactic1);
            tactic1.GetComponent<DraggableCard>().enabled = false;
            
            foreach (var card in player1DeckManager.currentPlayerDeck)
            {
                var tempCard = card.GetComponent<Card>();

                if (tempCard != null)
                {
                    tempCard.Owner = _player1.GetComponent<Hero>();
                
                    tempCard.OnCardInitialized -= () => ShowCardsWithSpell(true); // Это зачем? --------------------------------------------------
                    tempCard.OnCardInitialized += () => ShowCardsWithSpell(true);
                
                    tempCard.OnCardInitialized -= () => { UpdateVulnerability(tempCard); _initializedCardsCount += 1;
                        DeleteCardFromHand(card); UpdateVulnerabilityInBackSector(tempCard);
                    };
                    tempCard.OnCardInitialized += () => { UpdateVulnerability(tempCard); _initializedCardsCount += 1;
                        DeleteCardFromHand(card); UpdateVulnerabilityInBackSector(tempCard);
                    };

                    tempCard.OnCardWounded -= () => {UpdateVulnerabilityInBackSector(tempCard); };
                    tempCard.OnCardWounded += () => {UpdateVulnerabilityInBackSector(tempCard); };
                    
                    tempCard.OnCardDead -= () => { TransferToDiscard(card); };
                    tempCard.OnCardDead += () => { TransferToDiscard(card); };

                    tempCard.OnCardClicked -= CardClickHandle;
                    tempCard.OnCardClicked += CardClickHandle;
                    
                    tempCard.OnCardStateChanged -= battleLog.AddLogMessage;
                    tempCard.OnCardStateChanged += battleLog.AddLogMessage;
                }
                else
                {
                    var tempTacticCard = card.GetComponent<TacticCard>();
                    tempTacticCard.Owner = _player1.GetComponent<Hero>();

                    tempTacticCard.OnDisabled -= () => TransferToDiscard(card);
                    tempTacticCard.OnDisabled += () => TransferToDiscard(card);

                    tempTacticCard.OnInitialized -= () => DeleteCardFromHand(card);
                    tempTacticCard.OnInitialized += () => DeleteCardFromHand(card);
                }
            }
        }
        
        private void InitPlayer2()
        {
            _player2 = Instantiate(heroPrefab, _player2Sectors[1].transform.GetChild(1));
            _player2.transform.localScale = new Vector3(0.85f, 0.9f, 0.9f);
            _player2.AddComponent(_player2Hero.GetType());
            _player2.GetComponent<Hero>().balanceText = player2Balance;
            _player2.GetComponent<Card>().Sector = 1;
            UpdateVulnerability(_player2.GetComponent<Card>());
            _player2.GetComponent<Card>().OnCardClicked += CardClickHandle;
            _player2.GetComponent<Hero>().OnCardStateChanged += battleLog.AddLogMessage;

            var tactic2 = Instantiate(tacticCardPrefab, _player2Sectors[1].transform.GetChild(0));
            switch (_player2Hero.Tech)
            {
                case "Cyber":
                    tactic2.AddComponent(cyberCommonTactic.GetType());
                    break;
                case "Bio":
                    tactic2.AddComponent(bioCommonTactic.GetType());
                    break;
            }

            tactic2.GetComponent<TacticCard>().Owner = _player2.GetComponent<Hero>();
            tactic2.GetComponent<TacticCard>().Initialize();
            tactic2.GetComponent<TacticCard>().Sector = 1;
            tactic2.GetComponent<TacticCard>().OnDisabled -= () => TransferToDiscard(tactic2);
            tactic2.GetComponent<TacticCard>().OnDisabled += () => TransferToDiscard(tactic2);
            tactic2.GetComponent<TacticCard>().OnInitialized -= () => DeleteCardFromHand(tactic2);
            tactic2.GetComponent<TacticCard>().OnInitialized += () => DeleteCardFromHand(tactic2);
            tactic2.GetComponent<DraggableCard>().enabled = false;
            
            foreach (var card in player2DeckManager.currentPlayerDeck)
            {
                var tempCard = card.GetComponent<Card>();

                if (tempCard != null)
                {
                    tempCard.Owner = _player2.GetComponent<Hero>();
                
                    tempCard.OnCardInitialized -= () => ShowCardsWithSpell(true);
                    tempCard.OnCardInitialized += () => ShowCardsWithSpell(true);
                
                    tempCard.OnCardInitialized -= () => { UpdateVulnerability(tempCard); _initializedCardsCount += 1;
                        DeleteCardFromHand(card); UpdateVulnerabilityInBackSector(tempCard);
                    };
                    tempCard.OnCardInitialized += () => { UpdateVulnerability(tempCard); _initializedCardsCount += 1;
                        DeleteCardFromHand(card); UpdateVulnerabilityInBackSector(tempCard);
                    };

                    tempCard.OnCardWounded -= () => {UpdateVulnerabilityInBackSector(tempCard); };
                    tempCard.OnCardWounded += () => {UpdateVulnerabilityInBackSector(tempCard); };
                    
                    tempCard.OnCardDead -= () => { TransferToDiscard(card); };
                    tempCard.OnCardDead += () => { TransferToDiscard(card); };

                    tempCard.OnCardClicked -= CardClickHandle;
                    tempCard.OnCardClicked += CardClickHandle;
                    
                    tempCard.OnCardStateChanged -= battleLog.AddLogMessage;
                    tempCard.OnCardStateChanged += battleLog.AddLogMessage;
                }
                else
                {
                    var tempTacticCard = card.GetComponent<TacticCard>();
                    tempTacticCard.Owner = _player2.GetComponent<Hero>();

                    tempTacticCard.OnDisabled -= () => TransferToDiscard(card);
                    tempTacticCard.OnDisabled += () => TransferToDiscard(card);

                    tempTacticCard.OnInitialized -= () => DeleteCardFromHand(card);
                    tempTacticCard.OnInitialized += () => DeleteCardFromHand(card);
                }
            }
        }

        /*private void InitPlayer2()
        {
            _player2 = Instantiate(heroPrefab, _player2Sectors[1].transform.GetChild(1));
            _player2.AddComponent(player2Hero.GetType());
            _player2.GetComponent<Card>().Sector = 1;
            UpdateVulnerability(_player2.GetComponent<Card>());
            _player2.GetComponent<Card>().OnCardClicked += CardClickHandle;
            
            var tactic2 = Instantiate(tacticCardPrefab, _player2Sectors[1].transform.GetChild(0));
            tactic2.AddComponent(player2CommonTactic.GetType());
            tactic2.GetComponent<TacticCard>().Owner = _player2.GetComponent<Hero>();
            tactic2.GetComponent<TacticCard>().Initialize();
            tactic2.GetComponent<TacticCard>().Sector = 1;
            tactic2.GetComponent<DraggableCard>().enabled = false;
            
            foreach (var card in enemyDeckManager.currentEnemyDeck)
            {
                var tempCard = card.GetComponent<Card>();
                tempCard.Owner = _player2.GetComponent<Hero>();
                
                tempCard.OnCardInitialized -= () => { UpdateVulnerability(tempCard); _initializedCardsCount += 1;
                    DeleteCardFromHand(card); UpdateVulnerabilityInBackSector(tempCard);
                };
                tempCard.OnCardInitialized += () => { UpdateVulnerability(tempCard); _initializedCardsCount += 1;
                    DeleteCardFromHand(card); UpdateVulnerabilityInBackSector(tempCard);
                };

                tempCard.OnCardDead -= () => { TransferToDiscard(card); UpdateVulnerabilityInBackSector(tempCard); };
                tempCard.OnCardDead += () => { TransferToDiscard(card); UpdateVulnerabilityInBackSector(tempCard); };

                tempCard.OnCardClicked -= CardClickHandle;
                tempCard.OnCardClicked += CardClickHandle;
            }
        }*/

        private void UpdateVulnerability(Card card)
        {
            if (card.Side == 1)
            {
                if (card.Sector < 3) card.IsVulnerable = true;
                else
                {
                    var vanguardSector = card.Sector - 3;
                    var vanguardSectorCards = _player1Sectors[vanguardSector].transform.GetComponentsInChildren<Card>().Where(c => !c.isWounded).ToArray();
                    card.IsVulnerable = vanguardSectorCards.Length == 0;
                }
            }
            else
            {
                if (card.Sector > 2) card.IsVulnerable = true;
                else
                {
                    var vanguardSector = card.Sector + 3;
                    var vanguardSectorCards = _player2Sectors[vanguardSector].transform.GetComponentsInChildren<Card>().Where(c => !c.isWounded).ToArray();
                    card.IsVulnerable = vanguardSectorCards.Length == 0;
                }
            }
        }

        private void UpdateVulnerabilityInBackSector(Card card)
        {
            foreach (var tempCard in _battleField.GetComponentsInChildren<Card>().Where(c => c.Side == card.Side && c.IsActive))
            {
                UpdateVulnerability(tempCard);
            }
        }

        private void CardClickHandle(Card card)
        {
            var whosTurn = _turnNumber % 2 == 1 ? 1 : 2;
            
            if (_turnPhase == 1 && card.IsActive && card.IsCardWithActiveSpell && card.Side == whosTurn && !_isWaitingForTarget)
            {
                _isWaitingForTarget = true;
                _lastCardClicked = card;
                log.text = "Выберите цель способности";
            }

            else if (_turnPhase == 1 && _isWaitingForTarget && card.IsActive && card.IsVulnerable && card.Side != whosTurn)
            {
                ShowCardsWithSpell(false);
                _lastCardClicked?.ActiveSpell(card);
                if (!_lastCardClicked.IsCardWithActiveSpell || _lastCardClicked.IsCardWithActiveSpell && _lastCardClicked.Name == "Гнилозмей") log.text = $"Способность применена на карту: {card.Name}";
                else
                    log.text = "Выбрана некорректная цель";
                _isWaitingForTarget = false;
                _lastCardClicked = null;
                ShowCardsWithSpell(true);
            }

            else if (_turnPhase == 1 && _isWaitingForTarget && card.IsActive && card.Side == whosTurn)
            {
                ShowCardsWithSpell(false);
                if (card.SpellType == "Untargeted" && card == _lastCardClicked)
                {
                    card.ActiveSpell(card);
                    log.text = "Способность применена";
                }

                else if (_lastCardClicked?.SpellType == "TargetNeighbour" && card.Sector == _lastCardClicked.Sector && card != _lastCardClicked && card.Side == _lastCardClicked.Side
                         || _lastCardClicked?.SpellType == "TargetAllyCaster" && card != _lastCardClicked && card.Side == _lastCardClicked.Side && !card.IsCardWithActiveSpell && card.CardDescription != null && card.CardDescription.Contains("(акт)")
                         || _lastCardClicked?.SpellType == "TargetAlly" && card.Side == _lastCardClicked.Side)
                {
                    _lastCardClicked.ActiveSpell(card);
                    log.text = $"Способность применена на карту: {card.Name}";
                }

                else
                {
                    if (_lastCardClicked.IsCardWithActiveSpell) log.text = "Выбрана некорректная цель";
                }

                _isWaitingForTarget = false;
                _lastCardClicked = null;
                ShowCardsWithSpell(true);
            }
            
            else if (_turnPhase == 1 && _isWaitingForTarget && card.Side == whosTurn && card.isWounded)
            {
                ShowCardsWithSpell(false);
                if (_lastCardClicked.SpellType == "TargetAllyCorpse")
                {
                    _lastCardClicked.ActiveSpell(card);
                    log.text = "Способность применена";
                }
                else
                {
                    log.text = "Выбрана некорректная цель";
                }
                
                _isWaitingForTarget = false;
                _lastCardClicked = null;
                ShowCardsWithSpell(true);
            }
            
            else if (_turnPhase == 1 && _isWaitingForTarget && (!card.IsActive || !card.IsVulnerable || card.Side == whosTurn))
            {
                _isWaitingForTarget = false;
                _lastCardClicked = null;
            }

            else if (_turnPhase == 2 && card.IsVulnerable && card.IsActive && card.Side != whosTurn)
            {
                switch (whosTurn)
                {
                    case 1:
                        _player1.GetComponent<Hero>().Attack(card);
                        break;
                    case 2:
                        _player2.GetComponent<Hero>().Attack(card);
                        break;
                }
                _turnPhase += 1;
            }
        }

        private void ShowVulnerableCards(bool activity)
        {
            if (_turnNumber % 2 == 1) // Ход игрока 1
            {
                foreach (var sector in _player2Sectors)
                {
                    foreach (var card in sector.transform.GetComponentsInChildren<Card>().Where(c => c.IsVulnerable && c.IsActive))
                    {
                        var newColor = activity ? new Color(1, 0f, 0f, 0.7f) : new Color(1, 1, 1, 0f);
                        card.statusImage.color = newColor;
                    }
                }
            }
            else // Ход игрока 2
            {
                foreach (var sector in _player1Sectors)
                {
                    foreach (var card in sector.transform.GetComponentsInChildren<Card>().Where(c => c.IsVulnerable && c.IsActive))
                    {
                        var newColor = activity ? new Color(1, 0f, 0f, 0.7f) : new Color(1, 1, 1, 0f);
                        card.statusImage.color = newColor;
                    }
                }
            }
            

            _cardsShowed = activity;
        }
        
        private void ShowCardsWithSpell(bool activity)
        {
            if (_turnNumber % 2 == 1) // Ход игрока 1
            {
                foreach (var sector in _player1Sectors)
                {
                    foreach (var card in sector.transform.GetComponentsInChildren<Card>().Where(c => c.IsCardWithActiveSpell && c.IsActive))
                    {
                        var newColor = activity ? new Color(0.76f, 1, 0,0.7f) : new Color(1, 1, 1);
                        card.GetComponent<Image>().color = newColor;
                    }
                }
            }
            else // Ход игрока 2
            {
                foreach (var sector in _player2Sectors)
                {
                    foreach (var card in sector.transform.GetComponentsInChildren<Card>().Where(c => c.IsCardWithActiveSpell && c.IsActive))
                    {
                        var newColor = activity ? new Color(0.76f, 1, 0,0.7f) : new Color(1, 1, 1);
                        card.GetComponent<Image>().color = newColor;
                    }
                }
            }

            _spellsShowed = activity;
        }

        private void DeleteCardFromHand(GameObject card)
        {
            Debug.Log(card);
            if (_turnNumber % 2 == 1) // Ход игрока 1
            {
                player1HandManager.playerHand.Remove(card);
            }
            else // Ход игрока 2
            {
                player2HandManager.playerHand.Remove(card);
            }
        }

        private void TransferToDiscard(GameObject card)
        {
            if (card.GetComponent<Card>()?.Side == 1 || card.GetComponent<TacticCard>()?.Side == 1)
            {
                player1DiscardManager.AddCardToDiscard(card);
            }
            if (card.GetComponent<Card>()?.Side == 2 || card.GetComponent<TacticCard>()?.Side == 2)
            {
                player2DiscardManager.AddCardToDiscard(card);
            }
        }

        private void Flip(int flipCoef)
        {
            battleField.transform.localScale = new Vector3(1, flipCoef, 1);
            foreach (var sector in _sectors) sector.transform.localScale = new Vector3(1, flipCoef, 1);
            
            switch (flipCoef)
            {
                case -1: // Ход игрока 2
                {
                    foreach (var obj in _player1UI) obj.SetActive(false);
                    foreach (var obj in _player2UI) obj.SetActive(true);
                    break;
                }
                case 1: // Ход игрока 1
                {
                    foreach (var obj in _player1UI) obj.SetActive(true);
                    foreach (var obj in _player2UI) obj.SetActive(false);
                    break;
                }
            }
        }

        private void EndPhaseButtonUpdate(int turnPhase)
        {
            endPhaseButtonText.text = turnPhase switch
            {
                1 => "Перейти к атаке",
                2 => "Пропустить атаку",
                3 => "Завершить ход",
                _ => endPhaseButtonText.text
            };
        }

        public int GetTurnNumber()
        {
            return _turnNumber;
        }
        
        private void Update()
        {
            if (_lastCardClicked && !_isMarked)
            {
                marker.SetActive(true);
                marker.transform.position = _lastCardClicked.transform.position;
                _isMarked = true;
            }

            if (_isMarked && !_lastCardClicked)
            {
                marker.SetActive(false);
                _isMarked = false;
            }
            
            EndPhaseButtonUpdate(_turnPhase);
            
            if (_turnNumber == 0)
            {
                InitPlayer2();
                _turnNumber += 1;
                intermediateCanvas.gameObject.SetActive(true);
            }
            
            if (_giveUpButtonPressed || (_turnPhase != -1 && (_player1.GetComponent<Hero>().Health <= 0 || _player2.GetComponent<Hero>().Health <= 0)))
            {
                int winner;
                
                foreach (var canvas in FindObjectsByType<Canvas>((FindObjectsSortMode)FindObjectsInactive.Exclude))
                {
                    canvas.gameObject.SetActive(false);
                }
                endGameCanvas.gameObject.SetActive(true);
                if (_giveUpButtonPressed)
                {
                    winner = _turnNumber % 2 == 1 ? 2 : 1;
                }
                else
                {
                    winner = _player1.GetComponent<Hero>().Health <= 0 ? 2 : 1;
                }
                finalText.text = "Победил игрок " + winner;
                _turnPhase = -1;
                _giveUpButtonPressed = false;
            }
            
            else if (_turnNumber % 2 == 1) // Ход игрока 1
            {
                Flip(1);
                
                if (_turnPhase == 0) // Фаза добора карт
                { 
                    if (player1HandManager.playerHand.Count < 5) player1HandManager.GetTheCards();
                    foreach (var card in player1HandManager.playerHand)
                    {
                        card.GetComponent<DraggableCard>().enabled = true;
                    }
                    _turnPhase += 1;
                }

                if (_turnPhase == 1) // Фаза разыгрывания карт и использования способностей
                {
                    playedCardCount.text = _initializedCardsCount + "/3";
                    if (_initializedCardsCount == 3)
                    {
                        foreach (var card in player1HandManager.playerHand)
                        {
                            if (card.GetComponent<TacticCard>() == null) card.GetComponent<DraggableCard>().enabled = false;
                        }
                    }

                    if (!_spellsShowed) ShowCardsWithSpell(true);

                    if (_turnNumber == 1) endPhaseButtonText.text = "Завершить ход";
                }

                if (_turnPhase == 2 && _turnNumber != 1) // Фаза атаки
                {
                    marker.SetActive(false);
                    _isMarked = false;
                    
                    foreach (var card in player1HandManager.playerHand)
                    {
                        card.GetComponent<DraggableCard>().enabled = false;
                    }
                    
                    if (_spellsShowed) ShowCardsWithSpell(false);

                    if (!_cardsShowed) ShowVulnerableCards(true);
                }

                if (_turnPhase == 2 && _turnNumber == 1)
                {
                    if (_spellsShowed) ShowCardsWithSpell(false);
                    _turnPhase += 1;
                }

                if (_turnPhase == 3) // Завершающая фаза
                {
                    if (!_isUpdated)
                    {
                        foreach (var card in FindObjectsOfType<Card>().Where(c => c.Side == 1 && (c.IsActive || c.isWounded)))
                        {
                            card.HandleUpdate();
                        }
                        ShowVulnerableCards(false);
                        _isUpdated = true;
                    }
                    if (_turnNumber == 1) _turnPhase += 1;
                }

                if (_turnPhase == 4) // Передача хода
                {
                    _player1.GetComponent<Hero>().Balance = _player1.GetComponent<Hero>().MaxBalance;
                    _player1.GetComponent<Hero>().UpdateCardDisplay();
                    _initializedCardsCount = 0;
                    _isUpdated = false;
                    intermediateCanvas.gameObject.SetActive(true);
                    _turnNumber += 1;
                    _turnPhase = 0;
                }
            }
            
            else if (_turnNumber % 2 == 0) // Ход игрока 2
            {
                Flip(-1);
                
                if (_turnPhase == 0) // Фаза добора карт
                { 
                    if (player2HandManager.playerHand.Count < 5) player2HandManager.GetTheCards();
                    foreach (var card in player2HandManager.playerHand)
                    {
                        card.GetComponent<DraggableCard>().enabled = true;
                    }
                    _turnPhase += 1;
                }

                if (_turnPhase == 1) // Фаза разыгрывания карт и использования способностей
                {
                    playedCardCount.text = _initializedCardsCount + "/3";
                    if (_initializedCardsCount == 3)
                    {
                        foreach (var card in player2HandManager.playerHand)
                        {
                            if (card.GetComponent<TacticCard>() == null) card.GetComponent<DraggableCard>().enabled = false;
                        }
                    }

                    if (!_spellsShowed) ShowCardsWithSpell(true);
                }

                if (_turnPhase == 2) // Фаза атаки
                {
                    marker.SetActive(false);
                    _isMarked = false;
                    
                    foreach (var card in player2HandManager.playerHand)
                    {
                        card.GetComponent<DraggableCard>().enabled = false;
                    }
                    
                    if (_spellsShowed) ShowCardsWithSpell(false);

                    if (!_cardsShowed) ShowVulnerableCards(true);
                }
                
                if (_turnPhase == 3) // Завершающая фаза
                {
                    if (!_isUpdated)
                    {
                        foreach (var card in FindObjectsOfType<Card>().Where(c => c.Side == 2 && (c.IsActive || c.isWounded)))
                        {
                            card.HandleUpdate();
                        }
                        ShowVulnerableCards(false);
                        _isUpdated = true;
                    }
                }
                
                if (_turnPhase == 4) // Передача хода
                {
                    _player2.GetComponent<Hero>().Balance = _player2.GetComponent<Hero>().MaxBalance;
                    _player2.GetComponent<Hero>().UpdateCardDisplay();
                    _initializedCardsCount = 0;
                    _isUpdated = false;
                    intermediateCanvas.gameObject.SetActive(true);
                    _turnNumber += 1;
                    _turnPhase = 0;
                }
            }

            /*else if (_turnNumber % 2 == 0) // Ход соперника
            {
                if (_turnPhase == 0) // Фаза добора карт
                { 
                    enemyHandManager.GetTheCards();
                    _turnPhase += 1;
                }
                
                if (_turnPhase == 1) // Фаза разыгрывания карт и использования способностей
                {
                    for (var i = 0; i < 2; i++)
                    {
                        var sector = 1;
                        while (sector == 1)
                        {
                            sector = Random.Range(0, 6);
                        }
                        var card = Random.Range(0, enemyHandManager.enemyHand.Count);
                        if (enemyHandManager.enemyHand.Count != 0) _player2Sectors[sector].TryAddCard(enemyHandManager.enemyHand[card].GetComponent<Card>());
                    }

                    _turnPhase += 1;
                }
                
                if (_turnPhase == 2) // Фаза атаки
                {
                    var vulnerableCards = _battleField.GetComponentsInChildren<Card>()
                        .Where(c => c.Side == 1 && c.IsVulnerable).ToList();
                    var targetNum = Random.Range(0, vulnerableCards.Count);
                    _player2Hero.Attack(vulnerableCards[targetNum]);
                    log.text = "Враг выбрал целью атаки " + vulnerableCards[targetNum].Name;
                    _turnPhase += 1;
                }
            
                if (_turnPhase == 3) // Фаза сброса
                {
                    foreach (var card in FindObjectsOfType<Card>().Where(c => c.Side == 2 && c.IsActive))
                    {
                        card.HandleUpdate();
                    }
                    _initializedCardsCount = 0;
                    _turnNumber += 1;
                    _turnPhase = 0;
                }
            }*/
        }
    }
}
