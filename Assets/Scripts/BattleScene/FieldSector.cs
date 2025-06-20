using Cards;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleScene
{
    public class FieldSector : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Transform[] cardSlots = new Transform[3];
        private int _playerIndex;
        private int _sectorIndex;
    
        public void Initialize(int playerIdx, int sectionIdx)
        {
            _playerIndex = playerIdx;
            _sectorIndex = sectionIdx;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var card = eventData.pointerDrag?.GetComponent<Card>();
            var tacticCard = eventData.pointerDrag?.GetComponent<TacticCard>();
            if (card != null && _playerIndex == 1 && _sectorIndex != 4)
            {
                TryAddCard(card);
            }
            
            if (tacticCard != null && _playerIndex == 1 && _sectorIndex == 4)
            {
                TryAddTacticCard(tacticCard);
            }
        }

        private void TryAddTacticCard(TacticCard tacticCard)
        {
            foreach (var cardSlot in cardSlots)
            {
                if (cardSlot.GetComponentInChildren<TacticCard>() == null) continue;
                cardSlot.GetComponentInChildren<TacticCard>().Disable();
                tacticCard.transform.SetParent(cardSlot);
                tacticCard.transform.localPosition = Vector3.zero;
                tacticCard.Initialize();
                tacticCard.GetComponent<DraggableCard>().enabled = false;
                return;
            }
        }

        public void TryAddCard(Card card)
        {
            if (card.Owner.Balance < card.Cost) return;
            foreach (var cardSlot in cardSlots)
            {
                if (cardSlot.childCount != 0) continue;
                card.transform.SetParent(cardSlot);
                card.transform.localPosition = Vector3.zero;
                card.Initialize(_sectorIndex);
                card.GetComponent<DraggableCard>().enabled = false;
                return;
            }
        }
    }
}