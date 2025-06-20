using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class NewScreenButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Canvas targetCanvas;
        private void Start()
        {
            button.onClick.AddListener(ShowDeckCanvas);
        }

        private void ShowDeckCanvas()
        {
            GetComponentInParent<Canvas>().gameObject.SetActive(false);
            targetCanvas.gameObject.SetActive(true);
        }
    }
}
