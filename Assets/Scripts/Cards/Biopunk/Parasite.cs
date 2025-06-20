using System.Collections;
using System.Linq;
using UnityEngine;

namespace Cards.Biopunk
{
    public class Parasite : Card
    {
        private bool _isUpgraded;

        protected override void Awake()
        {
            Name = "Паразит";
            Cost = 4;
            Damage = 0;
            MaxHealth = 30;
            IsSpecial = true;
            Spell = "Мщение";
            CardDescription = "(пас) Каждый раз, когда союзный герой подвергается атаке, вражеский получает 5 урона";
            CheckUpgrades();
        }

        public override void Initialize(int targetSector)
        {
            base.Initialize(targetSector);
            Owner.DamageTaken -= Revenge;
            Owner.DamageTaken += Revenge;
        }

        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(186)) _isUpgraded = true;
        }

        private void Revenge()
        {
            if (!_isUpgraded) return;
            var enemyHero = FindObjectsByType<Hero>((FindObjectsSortMode)FindObjectsInactive.Exclude).Where(h => h.Side != Side).ToList();
            enemyHero[0].GetDamage(5);
            enemyHero[1].GetDamage(5);
        }

        protected override IEnumerator Die()
        {
            Owner.DamageTaken -= Revenge;
            yield return base.Die();
        }
    }
}