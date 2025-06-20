using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeckScreen
{
    public class CustomScrollRect : ScrollRect
    {
        // Отключаем обработку перетаскивания
        public override void OnBeginDrag(PointerEventData eventData) { }
        public override void OnDrag(PointerEventData eventData) { }
        public override void OnEndDrag(PointerEventData eventData) { }

        // Оставляем только прокрутку колёсиком
        public override void OnScroll(PointerEventData data)
        {
            base.OnScroll(data); // Стандартная прокрутка
        }
    }
}