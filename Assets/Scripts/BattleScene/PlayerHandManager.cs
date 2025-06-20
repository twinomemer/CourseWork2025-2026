using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BattleScene
{
    public class PlayerHandManager : MonoBehaviour
    {
        [SerializeField] private PlayerDeckManager playerDeck;
        [SerializeField] private DiscardManager playerDiscard;
        [SerializeField] private GridLayoutGroup playerHandGroup;
        
        public List<GameObject> playerHand = new List<GameObject>();

        public void GetTheCards()
        {
            if (playerDeck.currentPlayerDeck.Count == 0 && playerDiscard.currentPlayerDiscard.Count == 0) return;
            var amountNeeded = 5 - playerHand.Count;
            if (playerDeck.currentPlayerDeck.Count < amountNeeded)
            {
                for (var i = 0; i < playerDeck.currentPlayerDeck.Count; i++)
                {
                    var cardAmount = playerDeck.currentPlayerDeck.Count;
                    var n = Random.Range(0, cardAmount);
                    playerHand.Add(playerDeck.currentPlayerDeck[n]);
                    playerDeck.currentPlayerDeck[n].transform.SetParent(playerHandGroup.transform);
                    playerDeck.currentPlayerDeck.RemoveAt(n);
                }
                playerDeck.RefreshDeck();
                GetTheCards();
            }
            else
            {
                for (var i = 0; i < amountNeeded; i++)
                {
                    var cardAmount = playerDeck.currentPlayerDeck.Count;
                    var n = Random.Range(0, cardAmount);
                    playerHand.Add(playerDeck.currentPlayerDeck[n]);
                    playerDeck.currentPlayerDeck[n].transform.SetParent(playerHandGroup.transform);
                    playerDeck.currentPlayerDeck.RemoveAt(n);
                }
            }
        }
    }
}
