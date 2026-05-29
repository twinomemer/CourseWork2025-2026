using UnityEngine;
using UnityEngine.Audio;

namespace Sounds
{
    public class AudioSettings : MonoBehaviour
    {
        public static AudioSettings Instance { get; private set; }
        
        private float _musicVolume = 0.5f;
        private float _soundVolume = 0.5f;
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        private void Start()
        {
            ApplyVolumes();
        }
    
        private void ApplyVolumes()
        {
            SetMusicVolume(_musicVolume);
            SetSoundVolume(_soundVolume);
        }
    
        public void SetMusicVolume(float volume)
        {
            _musicVolume = volume;
        }
    
        public void SetSoundVolume(float volume)
        {
            _soundVolume = volume;
        }
    
        public float GetMusicVolume() => _musicVolume;
        public float GetSoundVolume() => _soundVolume;
    }
}