using System.Collections;
using UnityEngine;

namespace Cards.Biopunk
{
    public class Hydra : Card
    {
        private bool _isResistant = false;

        protected override void Awake()
        {
            Name = "Гидра";
            Tech = "Bio";
            Cost = 5;
            Damage = 10;
            MaxHealth = 10;
            IsSpecial = true;
            Spell = "Головы гидры";
            CardDescription = "(пас) Получает не более 5 урона за раз";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(198)) _isResistant = true;
        }

        public override void GetDamage(int damage)
        {
            if (_isResistant)
            {
                var actualDamage = damage - IncomingDamageReduction;
                if (actualDamage > 5) actualDamage = 5;
                damage = actualDamage;
            }
            base.GetDamage(damage);
        }
    }
}