using System;
using System.Linq;
using Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace DeckScreen
{
    public class DeckChecker : SoundManager
    {
        [SerializeField] private GameObject deckMessage;
        [SerializeField] private Button button;
        [SerializeField] private Canvas targetCanvas;
    
        private float _deactivationTime;
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        protected override void Start()
        {
            base.Start();
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
            PlaySound(0, destroyed:true);
            var tier5CardCount = 0;
                
            switch (IntersceneData.Instance.PlayerNum)
            {
                case 1:
                    tier5CardCount = IntersceneData.Instance.Player1Deck.Count(card => card.Cost == 5);

                    if (IntersceneData.Instance.Player1Deck.Count >= 12 && IntersceneData.Instance.Player1Deck.Count <= 20 && tier5CardCount < 2)
                    {
                        _canvas.gameObject.SetActive(false);
                        targetCanvas.gameObject.SetActive(true);
                    }
                    else
                    {
                        deckMessage.SetActive(true);
                        _deactivationTime = Time.time + 2f;
                    }
                    break;
                
                case 2:
                    tier5CardCount = IntersceneData.Instance.Player2Deck.Count(card => card.Cost == 5);
                    
                    if (IntersceneData.Instance.Player2Deck.Count >= 12 && IntersceneData.Instance.Player2Deck.Count <= 20 && tier5CardCount < 2)
                    {
                        _canvas.gameObject.SetActive(false);
                        targetCanvas.gameObject.SetActive(true);
                    }
                    else
                    {
                        deckMessage.SetActive(true);
                        _deactivationTime = Time.time + 2f;
                    }
                    break;
            }
            
        }
    }
}
