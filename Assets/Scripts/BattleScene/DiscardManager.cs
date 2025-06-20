using System;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScene
{
    public class DiscardManager : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup discardGrid;
        public List<GameObject> currentPlayerDiscard = new List<GameObject>();

        public void AddCardToDiscard(GameObject card)
        {
            if (!currentPlayerDiscard.Contains(card)) currentPlayerDiscard.Add(card);
            
            card.transform.SetParent(discardGrid.transform);
            card.transform.localPosition = new Vector3(0, 0, 0);
        }

        public void RemoveCardFromDiscard(GameObject card)
        {
            if (currentPlayerDiscard.Contains(card)) currentPlayerDiscard.Remove(card);
        }
    }
}
