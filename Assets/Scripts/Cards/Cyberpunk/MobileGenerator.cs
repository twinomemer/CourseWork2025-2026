using System.Collections;
using System.Linq;
using UnityEngine;

namespace Cards.Cyberpunk
{
    public class MobileGenerator : Card
    {
        protected override void Awake()
        {
            Name = "Передвижной генератор";
            Tech = "Cyber";
            Cost = 3;
            Damage = 0;
            MaxHealth = 15;
            IsSpecial = true;
            Spell = "Подозрительная энергетика";
            CardDescription = "(пас) Генерирует 1 очко энергии в ход. После смерти наносит 5 урона своим соседям по сектору";
            CheckUpgrades();
        }

        public override void Initialize(int targetSector)
        {
            base.Initialize(targetSector);
            Owner.MaxBalance += 1;
        }

        protected override IEnumerator Die()
        {
            Owner.MaxBalance -= 1;
            Owner.Balance -= 1;
            var cards = FindObjectsOfType<Card>();
            foreach (var card in cards)
            {
                if (card.IsActive && card.Side == Side && card.Sector == Sector && card != this)
                {
                    card.GetDamage(5);
                }
            }
            yield return base.Die();
        }
    }
}
