using System.Linq;
using UnityEngine;

namespace Cards.Biopunk
{
    public class Mimic : Card
    {
        protected override void Awake()
        {
            Name = "Доппельгангер";
            Tech = "Bio";
            Cost = 4;
            Damage = 0;
            MaxHealth = 25;
            IsSpecial = true;
            IsCardWithActiveSpell = true;
            Spell = "Подражание";
            SpellType = "Untargeted";
            CardDescription = "(акт) Перенимает 40% урона вражеского героя";
            CheckUpgrades();
        }

        public override void ActiveSpell(Card target)
        {
            if (!IsCardWithActiveSpell || target != this) return;
            var enemyHero = FindObjectsByType<Hero>((FindObjectsSortMode)FindObjectsInactive.Exclude).Where(hero => hero.Side != Side).ToList()[0];
            Damage = (int)(enemyHero.Damage * 0.4d);
            RecalculateDamage();
            Owner.Damage += AmplifiedDamage;
            Owner.RecalculateDamage();
            IsCardWithActiveSpell = false;
        }
    }
}