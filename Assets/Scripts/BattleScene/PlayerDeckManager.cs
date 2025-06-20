using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScene
{
    public class PlayerDeckManager : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup deckGrid;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private GameObject tacticCardPrefab;
        [SerializeField] private DiscardManager playerDiscard;
        public List<GameObject> currentPlayerDeck;

        private void Start()
        {
            foreach (var cardData in IntersceneData.Instance.PlayerDeck)
            {
                var cardObj = Instantiate(cardPrefab, deckGrid.transform);
                cardObj.AddComponent(cardData.GetType());
                cardObj.GetComponent<DraggableCard>().enabled = false;
                currentPlayerDeck.Add(cardObj);
            }

            foreach (var tacticCardData in IntersceneData.Instance.PlayerTacticDeck)
            {
                var cardObj = Instantiate(tacticCardPrefab, deckGrid.transform);
                cardObj.AddComponent(tacticCardData.GetType());
                cardObj.GetComponent<DraggableCard>().enabled = false;
                currentPlayerDeck.Add(cardObj);
            }
        }

        public void RefreshDeck()
        {
            foreach (var card in playerDiscard.currentPlayerDiscard.ToList())
            {
                currentPlayerDeck.Add(card);
                playerDiscard.RemoveCardFromDiscard(card);
                            
                card.transform.SetParent(deckGrid.transform);
                card.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}
