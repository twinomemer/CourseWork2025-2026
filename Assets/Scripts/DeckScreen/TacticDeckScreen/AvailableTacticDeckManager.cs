using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace DeckScreen.TacticDeckScreen
{
    public class AvailableTacticDeckManager : MonoBehaviour
    {
        public List<TacticCard> AvailableTacticDeck { get; private set; } = new List<TacticCard>();

        private void Start()
        {
            CheckDeck();
        }

        private void CheckDeck()
        {
            var cards = FindObjectsOfType<TacticCard>();
            switch (IntersceneData.Instance.PlayerNum)
            {
                case 1:
                    foreach (var card in cards)
                    {
                        if (!IntersceneData.Instance.Player1TacticDeck.Contains(card)) AddTacticCardToDeck(card);
                    }
                    break;
                
                case 2:
                    foreach (var card in cards)
                    {
                        if (!IntersceneData.Instance.Player2TacticDeck.Contains(card)) AddTacticCardToDeck(card);
                    }
                    break;
            }
        }
        public void AddTacticCardToDeck(TacticCard card)
        {
            AvailableTacticDeck.Add(card);
        }

        public void RemoveTacticCardFromDeck(TacticCard card)
        {
            if (AvailableTacticDeck.Contains(card)) AvailableTacticDeck.Remove(card);
        }

        public void ClearTacticDeck()
        {
            AvailableTacticDeck.Clear();
        }
    }
}
