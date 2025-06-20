using Cards;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DeckScreen.TacticDeckScreen
{
    public class TacticItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private RectTransform _rectTransform;
        private Canvas _mainCanvas;
        private CanvasGroup _canvasGroup;
        private int _siblingIndex;
        private Transform _parentToReturnTo;

        public bool placementIsCorrect;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _mainCanvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            placementIsCorrect = false;
            _parentToReturnTo = transform.parent;
            _siblingIndex = transform.GetSiblingIndex();
            transform.SetParent(transform.root); 
            transform.SetAsLastSibling();
            _canvasGroup.blocksRaycasts = false;
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!placementIsCorrect) transform.SetParent(_parentToReturnTo);
            transform.SetSiblingIndex(_siblingIndex);
            transform.localPosition = Vector3.zero;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}
