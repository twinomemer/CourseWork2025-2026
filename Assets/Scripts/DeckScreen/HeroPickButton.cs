using System;
using Cards;
using DeckScreen.TacticDeckScreen;
using Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace DeckScreen
{
    public class HeroPickButton : SoundManager
    {
        [SerializeField] private Canvas mainMenuCanvas;
        [SerializeField] private GridManager player1Grid;
        [SerializeField] private GridManager player1AvailableGrid;
        [SerializeField] private GridManager player2Grid;
        [SerializeField] private GridManager player2AvailableGrid;
        [SerializeField] private TacticGridManager player1TacticGrid;
        [SerializeField] private TacticGridManager player1AvailableTacticGrid;
        [SerializeField] private TacticGridManager player2TacticGrid;
        [SerializeField] private TacticGridManager player2AvailableTacticGrid;
        
        private Canvas _canvas;
        private Hero _hero;
        private Button _button;

        private void Awake()
        {
            _hero = GetComponent<Hero>();
            _canvas = GetComponentInParent<Canvas>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ChooseHero);
        }

        private void OnDisable()
        {
            Refresh();
        }

        private void ChooseHero()
        {
            PlaySound(0, destroyed:true);
            IntersceneData.Instance.PickHero(_hero);
            _canvas.gameObject.SetActive(false);
            mainMenuCanvas.gameObject.SetActive(true);
            if (IntersceneData.Instance.PlayerNum == 1)
            {
                IntersceneData.Instance.ClearDeck(1);
                player1Grid.ClearGrid();
                player1AvailableGrid.ClearGrid();
                player1AvailableGrid.DeckForming();
                player1TacticGrid.ClearGrid();
                player1AvailableTacticGrid.ClearGrid();
                player1AvailableTacticGrid.DeckForming();
            }
            else
            {
                IntersceneData.Instance.ClearDeck(2);
                player2Grid.ClearGrid();
                player2AvailableGrid.ClearGrid();
                player2AvailableGrid.DeckForming();
                player2TacticGrid.ClearGrid();
                player2AvailableTacticGrid.ClearGrid();
                player2AvailableTacticGrid.DeckForming();
            }
        }

        public void Refresh()
        {
            if (IntersceneData.Instance.Player1 is not null && IntersceneData.Instance.Player1 == _hero ||
                IntersceneData.Instance.Player2 is not null && IntersceneData.Instance.Player2 == _hero) _button.interactable = false;
            else _button.interactable = true;
        }
    }
}
