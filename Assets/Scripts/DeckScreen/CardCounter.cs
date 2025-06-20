using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeckScreen
{
    public class CardCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private GridLayoutGroup grid;

        private void Update()
        {
            countText.text = $"({grid.transform.childCount})";
        }
    }
}
