using UnityEngine;

namespace Sounds
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private Canvas settingsCanvas;

        private void Start()
        {
            musicSource.volume = AudioSettings.Instance.GetMusicVolume();
        }

        private void Update()
        {
            if (settingsCanvas && settingsCanvas.enabled) musicSource.volume = AudioSettings.Instance.GetMusicVolume();
        }
    }
}
