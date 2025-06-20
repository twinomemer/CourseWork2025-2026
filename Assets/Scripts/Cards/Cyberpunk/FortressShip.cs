using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cards.Cyberpunk
{
    public class FortressShip : Card
    {
        private int _missChance = 0;

        protected override void Awake()
        {
            Name = "Корабль-крепость";
            Cost = 5;
            Damage = 10;
            MaxHealth = 30;
            IsCardWithActiveSpell = false;
            IsSpecial = true;
            Spell = "Задавить числом\nУклонение";
            CardDescription = "(акт) Получает +1 к урону за каждую союзную карту на поле.\n(пас) Имеет 30% шанс избежать урона";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(109))
            {
                MaxHealth += 20;
                if (IntersceneData.Instance.PlayerUpgrades.Contains(106)) IsCardWithActiveSpell = true;
                if (IntersceneData.Instance.PlayerUpgrades.Contains(100)) _missChance = 30;
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
                    Debug.Log(card);
                }
            }
            var damageBonus = allyCards.Count - 1;
            Damage += damageBonus;
            HandleUpdate();
            IsCardWithActiveSpell = false;
        }
        
        public override void GetDamage(int damage)
        {
            var n = Random.Range(1, 101);
            if (n <= _missChance) return;
            base.GetDamage(damage);
        }
    }
}
