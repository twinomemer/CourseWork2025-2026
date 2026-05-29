using System;
using System.Linq;
using BattleScene;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cards.Biopunk.TacticCards
{
    public class Splitting : TacticCard
    {
        private void Awake()
        {
            Name = "Внезапная атака";
            Style = "TrickyTactic";
            Damage = 2;
            CardDescription = "Наносит 60% урона выбранной цели, а оставшиеся 20% одной случайной доступной карте";
        }

        public override void Attack(Card target)
        {
            target.GetDamage((int)(Owner.AmplifiedDamage * 0.6f));
            var battleField = target.GetComponentInParent<BattleField>();
            var allTargets = battleField.GetComponentsInChildren<Card>().Where(card => card.Side == target.Side && card.IsVulnerable && !card.isWounded);
            var enumerable = allTargets.ToList();
            var n = Random.Range(0, enumerable.Count);
            enumerable[n].GetDamage((int)Math.Truncate((decimal)(Owner.AmplifiedDamage * 0.2d)));
        }
    }
}
