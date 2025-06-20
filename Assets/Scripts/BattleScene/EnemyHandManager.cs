using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BattleScene
{
    public class EnemyHandManager : MonoBehaviour
    {
        [SerializeField] private EnemyDeckManager enemyDeck;
        [SerializeField] private DiscardManager enemyDiscard;
        
        public List<GameObject> enemyHand = new List<GameObject>();

        public void GetTheCards()
        {
            if (enemyDeck.currentEnemyDeck.Count == 0 && enemyDiscard.currentPlayerDiscard.Count == 0) return;
            var amountNeeded = 5 - enemyHand.Count;
            if (enemyDeck.currentEnemyDeck.Count < amountNeeded)
            {
                for (var i = 0; i < enemyDeck.currentEnemyDeck.Count; i++)
                {
                    var cardAmount = enemyDeck.currentEnemyDeck.Count;
                    var n = Random.Range(0, cardAmount);
                    enemyHand.Add(enemyDeck.currentEnemyDeck[n]);
                    enemyDeck.currentEnemyDeck.RemoveAt(n);
                }
                enemyDeck.RefreshDeck();
                GetTheCards();
            }
            else
            {
                for (var i = 0; i < amountNeeded; i++)
                {
                    var cardAmount = enemyDeck.currentEnemyDeck.Count;
                    var n = Random.Range(0, cardAmount);
                    enemyHand.Add(enemyDeck.currentEnemyDeck[n]);
                    enemyDeck.currentEnemyDeck.RemoveAt(n);
                }
            }
        }
    }
}
