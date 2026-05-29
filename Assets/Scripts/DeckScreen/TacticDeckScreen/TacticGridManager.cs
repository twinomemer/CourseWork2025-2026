using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DeckScreen.TacticDeckScreen
{
    public class TacticGridManager : MonoBehaviour, IDropHandler
    {
        [SerializeField] private bool isPlayerTacticDeck;
        [SerializeField] private AvailableTacticDeckManager availableTacticDeckManager;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button tacticButton;
        [SerializeField] private GameObject allTacticCardsCyberObject;
        [SerializeField] private GameObject allTacticCardsBioObject;
        [SerializeField] private GameObject tacticCardPrefab;
        [SerializeField] private int playerNum;
        
        private bool _isRefreshing;
        private TacticItem _item;
        private TacticCard[] _allTacticCards;

        private void Awake()
        {
            menuButton.onClick.AddListener(UpdateDecks);
            tacticButton.onClick.AddListener(UpdateDecks);
        }

        public void DeckForming()
        {
            if (!isPlayerTacticDeck)
            {
                switch (IntersceneData.Instance.PlayerNum)
                {
                    case 1:
                        switch (IntersceneData.Instance.Player1.Tech)
                        {
                            case "Bio":
                                _allTacticCards = allTacticCardsBioObject.GetComponents<TacticCard>();
                                allTacticCardsBioObject.SetActive(false);
                                break;
                            case "Cyber":
                                _allTacticCards = allTacticCardsCyberObject.GetComponents<TacticCard>();
                                allTacticCardsCyberObject.SetActive(false);
                                break;
                        }

                        break;
                    case 2:
                        switch (IntersceneData.Instance.Player2.Tech)
                        {
                            case "Bio":
                                _allTacticCards = allTacticCardsBioObject.GetComponents<TacticCard>();
                                allTacticCardsBioObject.SetActive(false);
                                break;
                            case "Cyber":
                                _allTacticCards = allTacticCardsCyberObject.GetComponents<TacticCard>();
                                allTacticCardsCyberObject.SetActive(false);
                                break;
                        }

                        break;
                }
                
                foreach (var cardData in _allTacticCards)
                {
                    var cardObj = Instantiate(tacticCardPrefab, transform);
                    cardObj.AddComponent(cardData.GetType());
                }
            }
        }
        
        private void Start()
        {
            if (isPlayerTacticDeck) MovePlayerCards();
        }
        
        private void MovePlayerCards()
        {
            var cards = FindObjectsByType<TacticCard>((FindObjectsSortMode)FindObjectsInactive.Exclude);
            var checkedCards = new List<TacticCard>();
            foreach (var card in cards)
            {
                foreach (var cardInDeck in IntersceneData.Instance.Player1TacticDeck)
                {
                    if (cardInDeck.Name != card.Name || checkedCards.Contains(card)) continue;
                    card.transform.SetParent(transform);
                    card.transform.localPosition = Vector3.zero;
                    checkedCards.Add(card);
                    break;
                }
            }
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            _item = eventData.pointerDrag.GetComponent<TacticItem>();
            
            if (!eventData.pointerDrag.CompareTag("Item")) return;
            _item.placementIsCorrect = true;
            var otherItemTransform = eventData.pointerDrag.transform;
            otherItemTransform.SetParent(transform);
            otherItemTransform.localPosition = Vector3.zero;
        }
        
        public void UpdateDecks()
        {
            if (isPlayerTacticDeck)
            {
                IntersceneData.Instance.ClearTacticDeck(playerNum);
                foreach (var card in GetComponentsInChildren<TacticCard>().ToList())
                {
                    IntersceneData.Instance.AddTacticCardToDeck(card, playerNum);
                }
            }

            if (!isPlayerTacticDeck)
            {
                availableTacticDeckManager.ClearTacticDeck();
                foreach (var card in GetComponentsInChildren<TacticCard>().ToList())
                {
                    availableTacticDeckManager.AddTacticCardToDeck(card);
                }
            }
        }
        
        public void ClearGrid()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}