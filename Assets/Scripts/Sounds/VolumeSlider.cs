using UnityEngine;
using UnityEngine.UI;

namespace Sounds
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private VolumeType volumeType;

        private enum VolumeType
        {
            Music,
            Sound
        }
    
        private void Start()
        {
            if (AudioSettings.Instance != null)
            {
                switch (volumeType)
                {
                    case VolumeType.Music:
                        slider.value = AudioSettings.Instance.GetMusicVolume();
                        break;
                    case VolumeType.Sound:
                        slider.value = AudioSettings.Instance.GetSoundVolume();
                        break;
                }
            }

            slider.onValueChanged.AddListener(OnVolumeChanged);
        }
    
        private void OnVolumeChanged(float value)
        {
            if (AudioSettings.Instance != null)
            {
                switch (volumeType)
                {
                    case VolumeType.Music:
                        AudioSettings.Instance.SetMusicVolume(value);
                        break;
                    case VolumeType.Sound:
                        AudioSettings.Instance.SetSoundVolume(value);
                        break;
                }
            }
        }
    }
}