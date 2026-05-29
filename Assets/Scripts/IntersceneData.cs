using System;
using System.Collections.Generic;
using Cards;
using UnityEngine;
using UnityEngine.Events;

public class IntersceneData : MonoBehaviour
{
    public static IntersceneData Instance { get; private set; }

    public int EnemyID { get; set; } = 1;
    public Hero Player1 { get; private set; }
    public Hero Player2 { get; private set; }
    public int PlayerNum { get; set; }
    public List<Card> Player1Deck { get; private set; } = new List<Card>();
    public List<Card> Player2Deck { get; private set; } = new List<Card>();
    public List<TacticCard> Player1TacticDeck { get; private set; } = new List<TacticCard>();
    public List<TacticCard> Player2TacticDeck { get; private set; } = new List<TacticCard>();
    public List<int> PlayerUpgrades { get; private set; } = new List<int>();

    private string _heroName1, _heroName2;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        for (var i = 0; i < 200; i++)
        {
            PlayerUpgrades.Add(i+1);
        }
    }

    public void AddCardToDeck(Card card, int playerNum)
    {
        switch (playerNum)
        {
            case 1: 
                Player1Deck.Add(card);
                break;
            case 2:
                Player2Deck.Add(card);
                break;
        }
        
    }

    public void AddTacticCardToDeck(TacticCard card, int playerNum)
    {
        switch (playerNum)
        {
            case 1: 
                Player1TacticDeck.Add(card);
                break;
            case 2:
                Player2TacticDeck.Add(card);
                break;
        }
    }
    
    public void RemoveCardFromDeck(Card card, int playerNum)
    {
        switch (playerNum)
        {
            case 1 when Player1Deck.Contains(card):
                Player1Deck.Remove(card);
                break;
            case 2 when Player2Deck.Contains(card):
                Player2Deck.Remove(card);
                break;
        }
    }
    public void RemoveTacticCardFromDeck(TacticCard card, int playerNum)
    {
        switch (playerNum)
        {
            case 1 when Player1TacticDeck.Contains(card):
                Player1TacticDeck.Remove(card);
                break;
            case 2 when Player2TacticDeck.Contains(card):
                Player2TacticDeck.Remove(card);
                break;
        }
    }
        
    public void AddUpgrade(int upgradeID)
    {
        PlayerUpgrades.Add(upgradeID);
    }

    public void ClearDeck(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                Player1Deck.Clear();
                break;
            case 2:
                Player2Deck.Clear();
                break;
        }
    }
    
    public void ClearTacticDeck(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                Player1TacticDeck.Clear();
                break;
            case 2:
                Player2TacticDeck.Clear();
                break;
        }
    }

    public void PickHero(Hero hero)
    {
        if (PlayerNum == 1)
        {
            Player1 = hero;
            _heroName1 = hero.Name;
        }
        else
        {
            Player2 = hero;
            _heroName2 = hero.Name;
        }
    }

    public void ResetHero()
    {
        if (PlayerNum == 1) Player1 = null;
        else Player2 = null;
    }
    
    public void ResetDeck()
    {
        if (PlayerNum == 1)
        {
            Player1Deck = new List<Card>();
            Player1TacticDeck = new List<TacticCard>();
        }
        else
        {
            Player2Deck = new List<Card>();
            Player2TacticDeck = new List<TacticCard>();
        }
    }
}