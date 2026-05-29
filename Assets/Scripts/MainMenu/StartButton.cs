using Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class StartButton : SoundManager
    {
        [SerializeField] private Button startButton; 
        [SerializeField] private GameObject deckMessage;

        private float _deactivationTime;
        protected override void Start()
        {
            base.Start();
            startButton.onClick.AddListener(CheckDeck);
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
            if (IntersceneData.Instance.Player1Deck.Count >= 1 && IntersceneData.Instance.Player2Deck.Count >= 1) SceneManager.LoadScene(1);
            else
            {
                deckMessage.SetActive(true);
                _deactivationTime = Time.time + 3f;
            }
        }
    }
}
