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
        [SerializeField] private PlayerDeckManager playerDeckManager;
        [SerializeField] private PlayerHandManager playerHandManager;
        [SerializeField] private EnemyDeckManager enemyDeckManager;
        [SerializeField] private EnemyHandManager enemyHandManager;
        [SerializeField] private DiscardManager playerDiscardManager;
        [SerializeField] private DiscardManager enemyDiscardManager;
        [SerializeField] private GameObject heroPrefab;
        [SerializeField] private GameObject tacticCardPrefab;
        [SerializeField] private Hero player1Hero;
        [SerializeField] private Hero player2Hero;
        [SerializeField] private TacticCard player1CommonTactic;
        [SerializeField] private TacticCard player2CommonTactic;
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Canvas endGameCanvas;
        [SerializeField] private TextMeshProUGUI log;
        [SerializeField] private TextMeshProUGUI finalText;
        [SerializeField] private TextMeshProUGUI playedCardCount;

        private bool _isWaitingForTarget = false, _cardsShowed = false, _spellsShowed = false, _giveUpButtonPressed = false;
        private int _turnNumber = 0;
        private int _turnPhase = 0;
        private int _initializedCardsCount = 0;
        private BattleField _battleField;
        private FieldSector[] _player1Sectors, _player2Sectors;
        private Card _lastCardClicked;
        private GameObject _player1, _player2;

        private void Awake()
        {
            endTurnButton.onClick.AddListener(() => _turnPhase += 1);
            giveUpButton.onClick.AddListener(() => _giveUpButtonPressed = true);
        }

        private void Start()
        {
            _battleField = FindAnyObjectByType<BattleField>();
            _player1Sectors = _battleField.transform.GetChild(0).GetComponentsInChildren<FieldSector>();
            _player2Sectors = _battleField.transform.GetChild(1).GetComponentsInChildren<FieldSector>();
            
            InitPlayer1();
        }

        private void InitPlayer1()
        {
            _player1 = Instantiate(heroPrefab, _player1Sectors[4].transform.GetChild(1));
            _player1.AddComponent(player1Hero.GetType());
            _player1.GetComponent<Card>().Sector = 4;
            UpdateVulnerability(_player1.GetComponent<Card>());
            _player1.GetComponent<Card>().OnCardClicked += CardClickHandle;

            var tactic1 = Instantiate(tacticCardPrefab, _player1Sectors[4].transform.GetChild(0));
            tactic1.AddComponent(player1CommonTactic.GetType());
            tactic1.GetComponent<TacticCard>().Owner = _player1.GetComponent<Hero>();
            tactic1.GetComponent<TacticCard>().Initialize();
            tactic1.GetComponent<TacticCard>().Sector = 4;
            tactic1.GetComponent<TacticCard>().OnDisabled -= () => TransferToDiscard(tactic1);
            tactic1.GetComponent<TacticCard>().OnDisabled += () => TransferToDiscard(tactic1);
            tactic1.GetComponent<DraggableCard>().enabled = false;
            
            foreach (var card in playerDeckManager.currentPlayerDeck)
            {
                var tempCard = card.GetComponent<Card>();

                if (tempCard != null)
                {
                    tempCard.Owner = _player1.GetComponent<Hero>();
                
                    tempCard.OnCardInitialized -= () => ShowCardsWithSpell(true);
                    tempCard.OnCardInitialized += () => ShowCardsWithSpell(true);
                
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
                tempCard.RescaleHealth((int) (tempCard.MaxHealth * 1.1f));
                
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
        }

        private void UpdateVulnerability(Card card)
        {
            if (card.Side == 1)
            {
                if (card.Sector < 3) card.IsVulnerable = true;
                else
                {
                    var vanguardSector = card.Sector - 3;
                    var vanguardSectorCards = _player1Sectors[vanguardSector].transform.GetComponentsInChildren<Card>();
                    card.IsVulnerable = vanguardSectorCards.Length == 0;
                }
            }
            else
            {
                if (card.Sector > 2) card.IsVulnerable = true;
                else
                {
                    var vanguardSector = card.Sector + 3;
                    var vanguardSectorCards = _player2Sectors[vanguardSector].transform.GetComponentsInChildren<Card>();
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
            if (_turnPhase == 1 && card.IsActive && card.IsCardWithActiveSpell && card.Side == 1 && !_isWaitingForTarget)
            {
                _isWaitingForTarget = true;
                _lastCardClicked = card;
                log.text = "Выберите цель способности";
            }

            else if (_turnPhase == 1 && _isWaitingForTarget && card.IsActive && card.IsVulnerable && card.Side == 2)
            {
                ShowCardsWithSpell(false);
                _lastCardClicked?.ActiveSpell(card);
                if (!_lastCardClicked.IsCardWithActiveSpell) log.text = $"Способность применена на карту: {card.Name}";
                else log.text = "Выбрана некорректная цель";
                _isWaitingForTarget = false;
                _lastCardClicked = null;
                ShowCardsWithSpell(true);
            }

            else if (_turnPhase == 1 && _isWaitingForTarget && card.IsActive && card.Side == 1)
            {
                ShowCardsWithSpell(false);
                if ((card.Name == "Хищник" || card.Name == "Корабль-крепость" ||
                     card.Name == "Тамбурмажор" || card.Name == "Техношаман") && card == _lastCardClicked)
                {
                    card.ActiveSpell(card);
                    log.text = "Способность применена";
                }

                else if (_lastCardClicked?.Name == "Кибер крыса" && card.Sector == _lastCardClicked.Sector &&
                    card != _lastCardClicked)
                {
                    _lastCardClicked.ActiveSpell(card);
                    log.text = "Способность применена";
                }
                else log.text = "Выбрана некорректная цель";
                _isWaitingForTarget = false;
                _lastCardClicked = null;
                ShowCardsWithSpell(true);
            }
            
            else if (_turnPhase == 1 && _isWaitingForTarget && (!card.IsActive || !card.IsVulnerable || card.Side == 1))
            {
                _isWaitingForTarget = false;
                _lastCardClicked = null;
            }

            else if (_turnPhase == 2 && card.IsVulnerable && card.IsActive && card.Side == 2)
            {
                player1Hero.Attack(card);
                _turnPhase += 1;
            }
        }

        private void ShowVulnerableCards(bool activity)
        {
            foreach (var sector in _player2Sectors)
            {
                foreach (var card in sector.transform.GetComponentsInChildren<Card>().Where(c => c.IsVulnerable))
                {
                    var newColor = activity ? new Color(1, 0.76f, 0.76f) : new Color(1, 1, 1);
                    card.GetComponent<Image>().color = newColor;
                }
            }

            _cardsShowed = activity;
        }
        
        private void ShowCardsWithSpell(bool activity)
        {
            foreach (var sector in _player1Sectors)
            {
                foreach (var card in sector.transform.GetComponentsInChildren<Card>().Where(c => c.IsCardWithActiveSpell))
                {
                    var newColor = activity ? new Color(0.76f, 1, 0,76f) : new Color(1, 1, 1);
                    card.GetComponent<Image>().color = newColor;
                }
            }

            _spellsShowed = activity;
        }

        private void DeleteCardFromHand(GameObject card)
        {
            foreach (var cardObject in playerHandManager.playerHand.ToList())
            {
                if (cardObject == card)
                {
                    playerHandManager.playerHand.Remove(cardObject);
                    return;
                }
            }

            foreach (var cardObject in enemyHandManager.enemyHand.ToList())
            {
                if (cardObject == card)
                {
                    enemyHandManager.enemyHand.Remove(cardObject);
                    return;
                }
            }
        }

        private void TransferToDiscard(GameObject card)
        {
            if (card.GetComponent<Card>()?.Side == 1 || card.GetComponent<TacticCard>()?.Side == 1)
            {
                playerDiscardManager.AddCardToDiscard(card);
            }
            if (card.GetComponent<Card>()?.Side == 2 || card.GetComponent<TacticCard>()?.Side == 2)
            {
                enemyDiscardManager.AddCardToDiscard(card);
            }
        }
        
        private void Update()
        {
            if (_turnNumber == 0)
            {
                InitPlayer2();
                _turnNumber += 1;
            }
            
            if (_giveUpButtonPressed || (_turnPhase != -1 && (_player1.GetComponent<Hero>().Health <= 0 || _player2.GetComponent<Hero>().Health <= 0)))
            {
                foreach (var canvas in FindObjectsByType<Canvas>((FindObjectsSortMode)FindObjectsInactive.Exclude))
                {
                    canvas.gameObject.SetActive(false);
                }
                endGameCanvas.gameObject.SetActive(true);
                var winner = _player1.GetComponent<Hero>().Health <= 0 || _giveUpButtonPressed ? "2" : "1";
                finalText.text = "Победил игрок " + winner;
                _turnPhase = -1;
                _giveUpButtonPressed = false;
            }
            
            else if (_turnNumber % 2 == 1) // Ход игрока
            {
                if (_turnPhase == 0) // Фаза добора карт
                { 
                    if (playerHandManager.playerHand.Count < 5) playerHandManager.GetTheCards();
                    foreach (var card in playerHandManager.playerHand)
                    {
                        card.GetComponent<DraggableCard>().enabled = true;
                    }
                    _turnPhase += 1;
                }

                if (_turnPhase == 1) // Фаза разыгрывания карт и использования способностей
                {
                    playedCardCount.text = _initializedCardsCount + "/2";
                    if (_initializedCardsCount == 2)
                    {
                        foreach (var card in playerHandManager.playerHand)
                        {
                            if (card.GetComponent<TacticCard>() == null) card.GetComponent<DraggableCard>().enabled = false;
                        }
                    }

                    if (!_spellsShowed) ShowCardsWithSpell(true);
                }

                if (_turnPhase == 2 && _turnNumber != 1) // Фаза атаки
                {
                    foreach (var card in playerHandManager.playerHand)
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

                if (_turnPhase == 3) // Фаза сброса
                {
                    foreach (var card in FindObjectsOfType<Card>().Where(c => c.Side == 1 && c.IsActive))
                    {
                        card.HandleUpdate();
                    }
                    ShowVulnerableCards(false);
                    _initializedCardsCount = 0;
                    _turnNumber += 1;
                    _turnPhase = 0;
                }
            }

            else if (_turnNumber % 2 == 0) // Ход соперника
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
                    player2Hero.Attack(vulnerableCards[targetNum]);
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
            }
        }
    }
}
