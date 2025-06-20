using System;
using System.Linq;
using BattleScene;
using UnityEngine;

namespace Cards.Biopunk.TacticCards
{
    public class Raid : TacticCard
    {
        private void Awake()
        {
            Name = "Налёт";
            Damage = 5;
            CardDescription = "Наносит 60% урона выбранной цели, а оставшийся урон делит между её соседями по сектору (если соседей нет, урон пропадёт)";
        }

        public override void Attack(Card target)
        {
            target.GetDamage((int) Math.Truncate(Owner.AmplifiedDamage * 0.6));
            
            var targetSector = target.GetComponentInParent<FieldSector>();
            var sideTargets = targetSector.GetComponentsInChildren<Card>().Where(card => card != target);
            var enumerable = sideTargets.ToList();
            if (enumerable.Count <= 0) return;
            var splittedDamage = (int) Math.Truncate((decimal)(Owner.AmplifiedDamage / enumerable.Count));
            if (splittedDamage < 1) splittedDamage = 1;
            foreach (var card in enumerable)
            {
                card.GetDamage(splittedDamage);
            }
        }
    }
}
