using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeckScreen
{
    public class GridManager : MonoBehaviour, IDropHandler
    {
        [SerializeField] private bool isPlayerDeck;
        [SerializeField] private AvailableDeckManager availableDeckManager;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button tacticButton;
        private bool _isRefreshing;
        private Item _item;

        private void Start()
        {
            if (isPlayerDeck) MovePlayerCards();
            menuButton.onClick.AddListener(UpdateDecks);
            tacticButton.onClick.AddListener(UpdateDecks);
        }
        
        private void MovePlayerCards()
        {
            var cards = FindObjectsByType<Card>((FindObjectsSortMode)FindObjectsInactive.Exclude);
            var checkedCards = new List<Card>();
            foreach (var card in cards)
            {
                foreach (var cardInDeck in IntersceneData.Instance.PlayerDeck)
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
            _item = eventData.pointerDrag.GetComponent<Item>();
            
            if (!eventData.pointerDrag.CompareTag("Item")) return;
            _item.placementIsCorrect = true;
            var otherItemTransform = eventData.pointerDrag.transform;
            otherItemTransform.SetParent(transform);
            otherItemTransform.localPosition = Vector3.zero;
        }

        public void UpdateDecks()
        {
            if (isPlayerDeck)
            {
                IntersceneData.Instance.ClearDeck();
                foreach (var card in GetComponentsInChildren<Card>().ToList())
                {
                    IntersceneData.Instance.AddCardToDeck(card);
                }
            }

            if (!isPlayerDeck)
            {
                availableDeckManager.ClearDeck();
                foreach (var card in GetComponentsInChildren<Card>().ToList())
                {
                    availableDeckManager.AddCardToDeck(card);
                }
            }
        }
    }
}