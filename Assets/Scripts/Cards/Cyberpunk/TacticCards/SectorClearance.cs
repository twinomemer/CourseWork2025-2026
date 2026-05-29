using System;
using System.Linq;
using BattleScene;
using UnityEngine;

namespace Cards.Cyberpunk.TacticCards
{
    public class SectorClearance : TacticCard
    {
        
        private void Awake()
        {
            Name = "Зачистка сектора";
            Style = "SplitTactic";
            Damage = 5;
            CardDescription = "Уменьшает урон на 25% и делит его поровну между всеми картами в выбранном секторе";
        }

        public override void Attack(Card target)
        {
            var targetSector = target.GetComponentInParent<FieldSector>();
            var allTargets = targetSector.GetComponentsInChildren<Card>().Where(card => card.Side == target.Side && card.IsVulnerable && card.IsActive).ToArray();
            var splittedDamage = (int) Math.Truncate((decimal)(Owner.AmplifiedDamage * 0.75f / allTargets.Length));
            foreach (var card in allTargets)
            {
                card.GetDamage(splittedDamage);
            }
        }
    }
}
