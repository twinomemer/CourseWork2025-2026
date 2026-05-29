using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeckScreen
{
    public class GridManager : MonoBehaviour, IDropHandler
    {
        public bool isPlayerDeck;
        public int playerNum;
        [SerializeField] private AvailableDeckManager availableDeckManager;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button tacticButton;
        [SerializeField] private GameObject allCardsCyberObject;
        [SerializeField] private GameObject allCardsBioObject;
        [SerializeField] private GameObject cardPrefab;
        
        private bool _isRefreshing;
        private Item _item;
        private Card[] _allCards;

        private void Awake()
        {
            menuButton.onClick.AddListener(UpdateDecks);
            tacticButton.onClick.AddListener(UpdateDecks);
        }

        public void DeckForming()
        {
            if (!isPlayerDeck)
            {
                switch (IntersceneData.Instance.PlayerNum)
                {
                    case 1:
                        switch (IntersceneData.Instance.Player1?.Tech)
                        {
                            case "Bio":
                                _allCards = allCardsBioObject.GetComponents<Card>();
                                allCardsBioObject.SetActive(false);
                                break;
                            case "Cyber":
                                _allCards = allCardsCyberObject.GetComponents<Card>();
                                allCardsCyberObject.SetActive(false);
                                break;
                        }

                        break;
                    case 2:
                        switch (IntersceneData.Instance.Player2?.Tech)
                        {
                            case "Bio":
                                _allCards = allCardsBioObject.GetComponents<Card>();
                                allCardsBioObject.SetActive(false);
                                break;
                            case "Cyber":
                                _allCards = allCardsCyberObject.GetComponents<Card>();
                                allCardsCyberObject.SetActive(false);
                                break;
                        }

                        break;
                }
                
                foreach (var cardData in _allCards)
                {
                    var cardObj = Instantiate(cardPrefab, transform);
                    cardObj.AddComponent(cardData.GetType());
                }
            }
        }

        private void OnEnable()
        {
            StartCoroutine(SortCoroutine());
        }

        private void Start()
        {
            SortCardsByCost();
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            _item = eventData.pointerDrag.GetComponent<Item>();
            
            if (!eventData.pointerDrag.CompareTag("Item")) return;
            _item.placementIsCorrect = true;
            var otherItemTransform = eventData.pointerDrag.transform;
            otherItemTransform.SetParent(transform);
            otherItemTransform.localPosition = Vector3.zero;
        }

        private void UpdateDecks()
        {
            if (isPlayerDeck)
            {
                IntersceneData.Instance.ClearDeck(playerNum);
                foreach (var card in GetComponentsInChildren<Card>().ToList())
                {
                    IntersceneData.Instance.AddCardToDeck(card, playerNum);
                }
            }
            else
            {
                availableDeckManager.ClearDeck();
                foreach (var card in GetComponentsInChildren<Card>().ToList())
                {
                    availableDeckManager.AddCardToDeck(card);
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
        
        private void SortCardsByCost()
        {
            var cards = GetComponentsInChildren<Card>().ToList();
            
            var sortedCards = cards
                .OrderByDescending(card => card.Cost)
                .ThenBy(card => card.Name)
                .ToList();
    
            for (var i = 0; i < sortedCards.Count; i++)
            {
                sortedCards[i].transform.SetSiblingIndex(i);
            }
        }

        private IEnumerator SortCoroutine()
        {
            yield return new WaitForEndOfFrame();
            SortCardsByCost();
        }
        
        private void OnTransformChildrenChanged()
        {
            SortCardsByCost();
        }
    }
}