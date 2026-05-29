using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cards.Cyberpunk
{
    public class TitanWarlord : Card
    {

        protected override void Awake()
        {
            Name = "Титан-воевода";
            Tech = "Cyber";
            Cost = 5;
            Damage = 15;
            MaxHealth = 40;
            IsCardWithActiveSpell = false;
            IsSpecial = true;
            Spell = "Задавить числом";
            SpellType = "Untargeted";
            CardDescription = "(акт) Получает +2 к урону за каждую активную союзную карту на поле.";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(109))
            {
                if (IntersceneData.Instance.PlayerUpgrades.Contains(106)) IsCardWithActiveSpell = true;
            }
        }

        public override void ActiveSpell(Card target)
        {
            if (!IsCardWithActiveSpell || target != this) return;
            var allyCards = new List<Card>();
            var cards = FindObjectsOfType<Card>();
            foreach (var card in cards)
            {
                if (card.IsActive && card.Side == Side)
                {
                    allyCards.Add(card);
                }
            }
            var damageBonus = allyCards.Count - 1;
            Damage += damageBonus * 2;
            HandleUpdate();
            IsCardWithActiveSpell = false;
        }
    }
}
