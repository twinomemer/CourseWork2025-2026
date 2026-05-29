using System;
using UnityEngine;

namespace Sounds
{
    public class SoundStopper : SoundManager
    {
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            base.Start();
        }

        protected override void Start() {}

        private void OnEnable()
        {
            audioSource.Stop();
            PlaySound(0);
        }
    }
}
