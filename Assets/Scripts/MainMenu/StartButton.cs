using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class StartButton : MonoBehaviour
    {
        [SerializeField] private Button startButton; 
        [SerializeField] private GameObject deckMessage;

        private float _deactivationTime;
        private void Start()
        {
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
            if (IntersceneData.Instance.PlayerDeck.Count >= 12) SceneManager.LoadScene(1);
            else
            {
                deckMessage.SetActive(true);
                _deactivationTime = Time.time + 3f;
            }
        }
    }
}
