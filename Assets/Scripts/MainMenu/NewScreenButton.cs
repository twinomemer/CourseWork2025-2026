using System;
using JetBrains.Annotations;
using Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class NewScreenButton : SoundManager
    {
        [SerializeField] private Button button;
        [SerializeField] private Canvas targetCanvas;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        protected override void Start()
        {
            base.Start();
            button.onClick.AddListener(ShowCanvas);
        }

        private void ShowCanvas()
        {
            PlaySound(0, destroyed:true);
            IntersceneData.Instance.PlayerNum = gameObject.name switch
            {
                "Player1HeroButton" => 1,
                "Player2HeroButton" => 2,
                _ => IntersceneData.Instance.PlayerNum
            };
            _canvas.gameObject.SetActive(false);
            targetCanvas.gameObject.SetActive(true);
            targetCanvas.sortingOrder = 1;
        }
    }
}
