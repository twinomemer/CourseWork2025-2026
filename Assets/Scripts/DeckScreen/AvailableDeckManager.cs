using System.Collections.Generic;
using Cards;
using NUnit.Framework;
using UnityEngine;

namespace DeckScreen
{
    public class AvailableDeckManager : MonoBehaviour
    {
        public List<Card> AvailableDeck { get; private set; } = new List<Card>();

        private void Start()
        {
            CheckDeck();
        }

        private void CheckDeck()
        {
            var cards = FindObjectsOfType<Card>();
            switch (IntersceneData.Instance.PlayerNum)
            {
                case 1:
                    foreach (var card in cards)
                    {
                        if (!IntersceneData.Instance.Player1Deck.Contains(card)) AddCardToDeck(card);
                    }
                    break;
                
                case 2:
                    foreach (var card in cards)
                    {
                        if (!IntersceneData.Instance.Player2Deck.Contains(card)) AddCardToDeck(card);
                    }
                    break;
            }
        }

        public void AddCardToDeck(Card card)
        {
            AvailableDeck.Add(card);
        }

        public void RemoveCardFromDeck(Card card)
        {
            if (AvailableDeck.Contains(card)) AvailableDeck.Remove(card);
        }

        public void ClearDeck()
        {
            AvailableDeck.Clear();
        }
    }
}
