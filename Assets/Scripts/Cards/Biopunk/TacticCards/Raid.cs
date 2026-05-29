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
            Style = "SplitTactic";
            Damage = 5;
            CardDescription = "Наносит 60% урона выбранной цели и по 15% урона её соседям по сектору. Если сосед один, он получит 30% урона, если соседей нет, урон пропадёт.";
        }

        public override void Attack(Card target)
        {
            target.GetDamage((int) Math.Truncate(Owner.AmplifiedDamage * 0.6));
            
            var targetSector = target.GetComponentInParent<FieldSector>();
            var sideTargets = targetSector.GetComponentsInChildren<Card>().Where(card => card != target && card.IsActive).ToArray();
            var enumerable = sideTargets.ToList();
            if (enumerable.Count <= 0) return;
            var splittedDamage = (int) Math.Truncate((decimal)(Owner.AmplifiedDamage * 0.3 / enumerable.Count));
            if (splittedDamage < 1) splittedDamage = 1;
            foreach (var card in enumerable)
            {
                card.GetDamage(splittedDamage);
            }
        }
    }
}
