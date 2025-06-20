using System;
using System.Linq;
using BattleScene;
using UnityEngine;

namespace Cards.Cyberpunk.TacticCards
{
    public class FireAtWill : TacticCard
    {
        
        private void Awake()
        {
            Name = "Огонь на поражение";
            Damage = 2;
            CardDescription = "Делит урон поровну между всеми доступными для атаки картами";
        }

        public override void Attack(Card target)
        {
            var battleField = target.GetComponentInParent<BattleField>();
            var allTargets = battleField.GetComponentsInChildren<Card>().Where(card => card.Side == target.Side && card.IsVulnerable);
            var splittedDamage = (int) Math.Truncate((decimal)(Owner.AmplifiedDamage / allTargets.Count()));
            if (splittedDamage < 1) splittedDamage = 1;
            foreach (var card in allTargets)
            {
                card.GetDamage(splittedDamage);
            }
        }
    }
}
