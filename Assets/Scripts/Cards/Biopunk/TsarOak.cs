using System.Collections;
using UnityEngine;

namespace Cards.Biopunk
{
    public class TsarOak : Card
    {
        protected override void Awake()
        {
            Name = "Царь-дуб";
            Tech = "Bio";
            Cost = 4;
            Damage = 20;
            MaxHealth = 30;
            IsSpecial = true;
            Spell = "Большая ответственность";
            CardDescription = "(пас) Отнимает у владельца 1 энергии в ход";
            CheckUpgrades();
        }

        public override void Initialize(int targetSector)
        {
            base.Initialize(targetSector);
            Owner.MaxBalance -= 1;
        }

        protected override IEnumerator Die()
        {
            Owner.MaxBalance += 1;
            Owner.Balance += 1;
            yield return base.Die();
        }
    }
}