using System;
using UnityEngine;

namespace Cards.Cyberpunk.TacticCards
{
    public class Diversion : TacticCard
    {
        private int _attackCount = 0;
        private void Awake()
        {
            Name = "Диверсия";
            Style = "TrickyTactic";
            Damage = 2;
            CardDescription = "Наносит 75% урона по цели. После трёх атак, вражеский герой потеряет 1 единицу энергии на 1 ход.";
        }

        public override void Attack(Card target)
        {
            target.GetDamage((int)Math.Truncate((decimal)(Owner.AmplifiedDamage * 0.75d)));
            _attackCount += 1;
            if (_attackCount == 3)
            {
                target.Owner.Balance -= 1;
                target.Owner.UpdateCardDisplay();
                _attackCount = 0;
            }
        }
    }
}
