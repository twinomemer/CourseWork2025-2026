using Cards;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DeckScreen
{
    public class Slot : MonoBehaviour, IDropHandler
    {
        private Item _item;
        
        public void OnDrop(PointerEventData eventData)
        {
            _item = eventData.pointerDrag.GetComponent<Item>();
            
            if (!eventData.pointerDrag.CompareTag("Item")) return;
            _item.placementIsCorrect = true;
            var otherItemTransform = eventData.pointerDrag.transform;
            otherItemTransform.SetParent(transform);
            otherItemTransform.localPosition = Vector3.zero;
        }
    }
}
