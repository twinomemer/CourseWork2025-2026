using System.Collections.Generic;
using Sounds;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Training
{
    public class ImageChanger : SoundManager
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        [SerializeField] private List<GameObject> images;

        private int _currentImage = 0;
        protected override void Start()
        {
            base.Start();
            nextButton.onClick.AddListener(NextImage);
            prevButton.onClick.AddListener(PreviousImage);
        }

        private void NextImage()
        {
            PlaySound(0, destroyed:true);
            images[_currentImage].SetActive(false);
            if (_currentImage == images.Count - 1) _currentImage = -1;
            images[++_currentImage].SetActive(true);
        }
        
        private void PreviousImage()
        {
            PlaySound(0, destroyed:true);
            images[_currentImage].SetActive(false);
            if (_currentImage == 0) _currentImage = images.Count;
            images[--_currentImage].SetActive(true);
        }
    }
}
