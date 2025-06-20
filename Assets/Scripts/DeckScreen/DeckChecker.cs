using UnityEngine;
using UnityEngine.UI;

namespace DeckScreen
{
    public class DeckChecker : MonoBehaviour
    {
        [SerializeField] private GameObject deckMessage;
        [SerializeField] private Button button;
        [SerializeField] private Canvas targetCanvas;
    
        private float _deactivationTime;
    
        private void Start()
        {
            button.onClick.AddListener(CheckDeck);
        }
        
        private void Update()
        {
            if (deckMessage.activeSelf && Time.time >= _deactivationTime)
            {
                deckMessage.SetActive(false);
            }
        }
        
        private void CheckDeck()
        {
            if (IntersceneData.Instance.PlayerDeck.Count >= 12)
            {
                GetComponentInParent<Canvas>().gameObject.SetActive(false);
                targetCanvas.gameObject.SetActive(true);
            }
            else
            {
                deckMessage.SetActive(true);
                _deactivationTime = Time.time + 2f;
            }
        }
    }
}
