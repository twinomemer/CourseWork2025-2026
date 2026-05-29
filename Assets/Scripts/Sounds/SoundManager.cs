using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sounds
{
    public class SoundManager : MonoBehaviour
    {
        public List<AudioClip> sounds;
        public List<SoundArrays> randSounds;

        protected AudioSource DestroyingSoundsSource;
        protected AudioSource AudioSource;

        protected virtual void Start()
        {
            DestroyingSoundsSource = GameObject.FindWithTag("DSSource").GetComponent<AudioSource>();
            AudioSource = GetComponent<AudioSource>();
            if (AudioSource == null)
            {
                gameObject.AddComponent<AudioSource>();
                AudioSource = GetComponent<AudioSource>();
            }
        }

        public void PlaySound(int index, bool random = false, bool destroyed = false)
        {
            var soundVolume = AudioSettings.Instance.GetSoundVolume();
            
            var clip = random ? randSounds[index].soundArray[Random.Range(0, randSounds[index].soundArray.Count)] : sounds[index];

            if (!AudioSource.isPlaying)
            {
                if (destroyed) DestroyingSoundsSource.PlayOneShot(clip, soundVolume);
                else AudioSource.PlayOneShot(clip, soundVolume);
            }
        }

        [Serializable]
        public class SoundArrays
        {
            public List<AudioClip> soundArray;
        }
    }
}
