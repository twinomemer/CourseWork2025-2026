using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards.Cyberpunk
{
    public class Tambourmajor : Card
    {
        private int _damageBuff = 2;

        protected override void Awake()
        {
            Name = "Тамбурмажор";
            Cost = 2;
            Damage = 0;
            MaxHealth = 16;
            IsCardWithActiveSpell = false;
            IsSpecial = true;
            Spell = "Гремит барабан";
            CardDescription = "(акт) Увеличивает урон соседей по сектору на 2";
            CheckUpgrades();
        }
        
        private void Buff(Card target)
        {
            target.Damage += _damageBuff;
            Owner.Damage -= target.AmplifiedDamage;
            target.RecalculateDamage();
            Owner.Damage += target.AmplifiedDamage;
            Owner.RecalculateDamage();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(58)) IsCardWithActiveSpell = true;
        }

        public override void ActiveSpell(Card target)
        {
            if (target != this) return;
            if (IsCardWithActiveSpell)
            {
                var cards = FindObjectsOfType<Card>();
                foreach (var card in cards)
                {
                    if (card.IsActive && card.Side == Side && card.Sector == Sector && card != this)
                    {
                        Buff(card);
                    }
                }
            }
            IsCardWithActiveSpell = false;
        }
    }
}
