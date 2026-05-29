using Sounds;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BattleScene.Buttons
{
    public class DeckButton : SoundManager
    {
        [SerializeField] private Canvas targetCanvas;
        [SerializeField] private Button button;
        [SerializeField] private Canvas currentCanvas;

        protected override void Start()
        {
            base.Start();
            button.onClick.AddListener(ShowCanvas);
        }

        private void ShowCanvas()
        {
            PlaySound(0, destroyed:true);
            if (!targetCanvas.isActiveAndEnabled) targetCanvas.gameObject.SetActive(true);
            targetCanvas.sortingOrder = 1;
            currentCanvas.sortingOrder = 0;
        }
    }
}
