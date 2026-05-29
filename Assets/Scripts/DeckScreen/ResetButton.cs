using System;
using Cards;
using DeckScreen.TacticDeckScreen;
using Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace DeckScreen
{
    public class ResetButton : SoundManager
    {
        [SerializeField] private int buttonType;
        [SerializeField] private Button button;
        [SerializeField] private HeroPickButton[] heroButtons;
        [SerializeField] private GridManager player1AvailableDeck;
        [SerializeField] private GridManager player2AvailableDeck;
        [SerializeField] private GridManager player1Deck;
        [SerializeField] private GridManager player2Deck;
        [SerializeField] private TacticGridManager player1AvailableTacticDeck;
        [SerializeField] private TacticGridManager player2AvailableTacticDeck;
        [SerializeField] private TacticGridManager player1TacticDeck;
        [SerializeField] private TacticGridManager player2TacticDeck;

        private void Awake()
        {
            switch (buttonType)
            {
                case 1:
                    button.onClick.AddListener(() => { IntersceneData.Instance.ResetHero(); IntersceneData.Instance.ResetDeck(); RefreshDeck();});
                    foreach (var hero in heroButtons)
                    {
                        button.onClick.AddListener(hero.Refresh);
                    }
                    break;
                
                case 2:
                    button.onClick.AddListener(() => { IntersceneData.Instance.ResetDeck(); RefreshDeck(); });
                    break;
            }
        }

        private void RefreshDeck()
        {
            PlaySound(0, destroyed:true);
            if (IntersceneData.Instance.PlayerNum == 1)
            {
                player1AvailableDeck.ClearGrid();
                player1Deck.ClearGrid();
                player1AvailableTacticDeck.ClearGrid();
                player1TacticDeck.ClearGrid();
                if (buttonType == 2)
                {
                    player1AvailableDeck.DeckForming();
                    player1AvailableTacticDeck.DeckForming();
                }
            }
            else
            {
                player2AvailableDeck.ClearGrid();
                player2Deck.ClearGrid();
                player2AvailableTacticDeck.ClearGrid();
                player2TacticDeck.ClearGrid();
                if (buttonType == 2)
                {
                    player2AvailableDeck.DeckForming();
                    player2AvailableTacticDeck.DeckForming();
                }
            }
        }
    }
}
