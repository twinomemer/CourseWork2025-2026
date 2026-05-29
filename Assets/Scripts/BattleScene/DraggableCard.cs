using Cards;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleScene
{
    public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Transform _originalParent;
        private Canvas _mainCanvas;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _mainCanvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            _originalParent = transform.parent;
            transform.SetParent(transform.root);
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            _canvasGroup.blocksRaycasts = true;
        
            if (transform.parent == transform.root)
            {
                ReturnToOriginalPosition();
            }
        }

        private void ReturnToOriginalPosition()
        {
            transform.SetParent(_originalParent);
            transform.localPosition = Vector3.zero;
        }
    }
}