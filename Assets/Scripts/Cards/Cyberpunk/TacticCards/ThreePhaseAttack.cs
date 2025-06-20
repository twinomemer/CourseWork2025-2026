using System;
using UnityEngine;

namespace Cards.Cyberpunk.TacticCards
{
    public class ThreePhaseAttack : TacticCard
    {
        
        private void Awake()
        {
            Name = "Двухфазовая атака";
            Damage = 2;
            CardDescription = "Уменьшает ваш урон на 40%, но наносит его дважды по одной выбранной цели";
        }

        public override void Attack(Card target)
        {
            var splittedDamage = (int)Math.Truncate((decimal)(Owner.AmplifiedDamage * 0.6d));
            target.GetDamage(splittedDamage);
            target.GetDamage(splittedDamage);
        }
    }
}
