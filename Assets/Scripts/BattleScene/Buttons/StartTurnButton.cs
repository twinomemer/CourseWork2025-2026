using Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScene.Buttons
{
    public class StartTurnButton : SoundManager
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Canvas canvas;

        protected override void Start()
        {
            base.Start();
            startButton.onClick.AddListener(StartTurn);
        }

        private void StartTurn()
        {
            PlaySound(0, destroyed: true);
            canvas.gameObject.SetActive(false);
        } 
    }
}
