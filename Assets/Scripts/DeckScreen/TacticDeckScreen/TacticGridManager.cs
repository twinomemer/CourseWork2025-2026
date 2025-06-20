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
        private bool _isRefreshing;
        private TacticItem _item;

        private void Start()
        {
            if (isPlayerTacticDeck) MovePlayerCards();
            menuButton.onClick.AddListener(UpdateDecks);
            tacticButton.onClick.AddListener(UpdateDecks);
        }
        
        private void MovePlayerCards()
        {
            var cards = FindObjectsByType<TacticCard>((FindObjectsSortMode)FindObjectsInactive.Exclude);
            var checkedCards = new List<TacticCard>();
            foreach (var card in cards)
            {
                foreach (var cardInDeck in IntersceneData.Instance.PlayerTacticDeck)
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
                IntersceneData.Instance.ClearTacticDeck();
                foreach (var card in GetComponentsInChildren<TacticCard>().ToList())
                {
                    IntersceneData.Instance.AddTacticCardToDeck(card);
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
    }
}