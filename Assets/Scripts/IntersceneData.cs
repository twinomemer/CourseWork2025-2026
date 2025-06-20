using System;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class IntersceneData : MonoBehaviour
{
    public static IntersceneData Instance { get; private set; }

    public int EnemyID { get; set; } = 1;
    public List<Card> PlayerDeck { get; private set; } = new List<Card>();
    public List<TacticCard> PlayerTacticDeck { get; private set; } = new List<TacticCard>();
    public List<int> PlayerUpgrades { get; private set; } = new List<int>();

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

    public void AddCardToDeck(Card card)
    {
        PlayerDeck.Add(card);
    }

    public void AddTacticCardToDeck(TacticCard card)
    {
        PlayerTacticDeck.Add(card);
    }
    
    public void RemoveCardFromDeck(Card card)
    {
        if (PlayerDeck.Contains(card)) PlayerDeck.Remove(card);
    }
    public void RemoveTacticCardFromDeck(TacticCard card)
    {
        if (PlayerTacticDeck.Contains(card)) PlayerTacticDeck.Remove(card);
    }
        
    public void AddUpgrade(int upgradeID)
    {
        PlayerUpgrades.Add(upgradeID);
    }

    public void ClearDeck()
    {
        PlayerDeck.Clear();
    }
    
    public void ClearTacticDeck()
    {
        PlayerTacticDeck.Clear();
    }
}