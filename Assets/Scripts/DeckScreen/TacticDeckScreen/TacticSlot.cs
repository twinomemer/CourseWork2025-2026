using UnityEngine;
using UnityEngine.EventSystems;

namespace DeckScreen.TacticDeckScreen
{
    public class TacticSlot : MonoBehaviour, IDropHandler
    {
        private TacticItem _item;
        
        public void OnDrop(PointerEventData eventData)
        {
            _item = eventData.pointerDrag.GetComponent<TacticItem>();
            
            if (!eventData.pointerDrag.CompareTag("Item")) return;
            _item.placementIsCorrect = true;
            var otherItemTransform = eventData.pointerDrag.transform;
            otherItemTransform.SetParent(transform);
            otherItemTransform.localPosition = Vector3.zero;
        }
    }
}
