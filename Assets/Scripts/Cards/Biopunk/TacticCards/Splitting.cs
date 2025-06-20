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
            Damage = 2;
            CardDescription = "Наносит урон выбранной цели, а также ещё половину от него одной случайной доступной карте";
        }

        public override void Attack(Card target)
        {
            target.GetDamage(Owner.AmplifiedDamage);
            var battleField = target.GetComponentInParent<BattleField>();
            var allTargets = battleField.GetComponentsInChildren<Card>().Where(card => card.Side == target.Side && card.IsVulnerable);
            var enumerable = allTargets.ToList();
            var n = Random.Range(0, enumerable.Count);
            enumerable[n].GetDamage((int)Math.Truncate((decimal)(Owner.AmplifiedDamage * 0.5d)));
        }
    }
}
