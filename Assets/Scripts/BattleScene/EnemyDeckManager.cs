using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Biopunk;
using Cards.Cyberpunk;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScene
{
    public class EnemyDeckManager : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private GridLayoutGroup enemyDeckContainer;
        [SerializeField] private DiscardManager enemyDiscard;
        public List<GameObject> currentEnemyDeck;
        private List<Card> _enemyDeck = new List<Card>();

        private void Start()
        {
            _enemyDeck = IntersceneData.Instance.Player2Deck;
            foreach (var cardData in _enemyDeck)
            {
                var cardObj = Instantiate(cardPrefab, enemyDeckContainer.transform);
                cardObj.AddComponent(cardData.GetType());
                cardObj.GetComponent<DraggableCard>().enabled = false;
                currentEnemyDeck.Add(cardObj);
            }
        }

        public void RefreshDeck()
        {
            foreach (var card in enemyDiscard.currentPlayerDiscard.ToList())
            {
                if (!currentEnemyDeck.Contains(card)) currentEnemyDeck.Add(card);
                enemyDiscard.RemoveCardFromDiscard(card);
                            
                card.transform.SetParent(enemyDeckContainer.transform);
                card.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}
