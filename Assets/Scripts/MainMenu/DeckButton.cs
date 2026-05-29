using System;
using Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class DeckButton : SoundManager
    {
        [SerializeField] private GameObject message;
        [SerializeField] private Button button;
        [SerializeField] private Canvas targetCanvas;
        [SerializeField] private int playerNum;
    
        private float _deactivationTime;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        protected override void Start()
        {
            base.Start();
            button.onClick.AddListener(CheckHeroPick);
        }
        
        private void Update()
        {
            if (message.activeSelf && Time.time >= _deactivationTime)
            {
                message.SetActive(false);
            }
        }
        
        private void CheckHeroPick()
        {
            PlaySound(0, destroyed:true);
            switch (playerNum)
            {
                case 1:
                    if (IntersceneData.Instance.Player1 == null)
                    {
                        message.SetActive(true);
                        _deactivationTime = Time.time + 2f;
                    }
                    else
                    {
                        IntersceneData.Instance.PlayerNum = playerNum;
                        _canvas.gameObject.SetActive(false);
                        targetCanvas.gameObject.SetActive(true);
                    }
                    break;
                case 2:
                    if (IntersceneData.Instance.Player2 == null)
                    {
                        message.SetActive(true);
                        _deactivationTime = Time.time + 2f;
                    }
                    else
                    {
                        IntersceneData.Instance.PlayerNum = playerNum;
                        _canvas.gameObject.SetActive(false);
                        targetCanvas.gameObject.SetActive(true);
                    }
                    break;
            }
        }
    }
}