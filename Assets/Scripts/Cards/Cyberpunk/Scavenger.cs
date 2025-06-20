using System.Collections;
using System.Linq;
using UnityEngine;

namespace Cards.Cyberpunk
{
    public class Scavenger : Card
    {
        private bool _isUpgraded = false;

        protected override void Awake()
        {
            Name = "Мусорщик";
            Cost = 1;
            Damage = 1;
            MaxHealth = 4;
            IsSpecial = true;
            Spell = "Взрыв";
            CardDescription = "(пас) При смерти наносит 5 урона вражескому герою";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(24)) _isUpgraded = true;
        }

        protected override IEnumerator Die()
        {
            if (_isUpgraded)
            {
                var enemyHero = FindObjectsByType<Hero>((FindObjectsSortMode)FindObjectsInactive.Exclude).Where(h => h.Side != Side).ToList();
                enemyHero[0].GetDamage(5);
                enemyHero[1].GetDamage(5);
            }
            yield return base.Die();
        }
    }
}
