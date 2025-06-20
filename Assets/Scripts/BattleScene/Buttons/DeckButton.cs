using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BattleScene.Buttons
{
    public class DeckButton : MonoBehaviour
    {
        [SerializeField] private Canvas targetCanvas;
        [SerializeField] private Button button;
        [SerializeField] private Canvas currentCanvas;

        private void Start()
        {
            button.onClick.AddListener(ShowCanvas);
        }

        private void ShowCanvas()
        {
            targetCanvas.sortingOrder = 1;
            currentCanvas.sortingOrder = 0;
        }
    }
}
