using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.Cyberpunk
{
    public class TechnoShaman : Card
    {
        private int _healthBuff = 5;

        protected override void Awake()
        {
            Name = "Техношаман";
            Cost = 2;
            Damage = 0;
            MaxHealth = 10;
            IsCardWithActiveSpell = true;
            IsSpecial = true;
            Spell = "Сомкнуть ряды";
            CardDescription = "(акт) Увеличивает здоровье соседей по сектору на 5";
            CheckUpgrades();
        }
        
        private void Buff(Card target)
        {
            target.RescaleHealth(target.MaxHealth + _healthBuff);
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(47)) IsCardWithActiveSpell = true;
        }

        public override void ActiveSpell(Card target)
        {
            if (target != this) return;
            if (IsCardWithActiveSpell)
            {
                var _buffedCards = new List<Card>();
                var cards = FindObjectsOfType<Card>();
                foreach (var card in cards)
                {
                    if (card.IsActive && card.Side == Side && card.Sector == Sector && card != this)
                    {
                        Buff(card);
                        _buffedCards.Add(card);
                    }
                }
            }
            IsCardWithActiveSpell = false;
        }
    }
}
