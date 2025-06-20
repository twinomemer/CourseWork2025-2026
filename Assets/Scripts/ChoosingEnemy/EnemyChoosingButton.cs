using System;
using Cards;
using UnityEngine;
using UnityEngine.UI;

namespace ChoosingEnemy
{
    public class EnemyChoosingButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private int enemyID;
        [SerializeField] private Canvas mainMenuCanvas;

        private void Awake()
        {
            button.onClick.AddListener(ChooseEnemy);
        }

        private void ChooseEnemy()
        {
            IntersceneData.Instance.EnemyID = enemyID;
            GetComponentInParent<Canvas>().gameObject.SetActive(false);
            mainMenuCanvas.gameObject.SetActive(true);
        }
    }
}
