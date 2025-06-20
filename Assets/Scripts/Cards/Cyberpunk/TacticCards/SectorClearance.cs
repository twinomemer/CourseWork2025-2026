using System;
using BattleScene;
using UnityEngine;

namespace Cards.Cyberpunk.TacticCards
{
    public class SectorClearance : TacticCard
    {
        
        private void Awake()
        {
            Name = "Зачистка сектора";
            Damage = 3;
            CardDescription = "Делит урон поровну между всеми картами в выбранном секторе";
        }

        public override void Attack(Card target)
        {
            var targetSector = target.GetComponentInParent<FieldSector>();
            var allTargets = targetSector.GetComponentsInChildren<Card>();
            var splittedDamage = (int) Math.Truncate((decimal)(Owner.AmplifiedDamage / allTargets.Length));
            foreach (var card in allTargets)
            {
                card.GetDamage(splittedDamage);
            }
        }
    }
}
