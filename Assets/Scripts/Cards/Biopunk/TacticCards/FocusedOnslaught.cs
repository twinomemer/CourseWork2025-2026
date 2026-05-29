using UnityEngine;

namespace Cards.Biopunk.TacticCards
{
    public class FocusedOnslaught : TacticCard
    {
        private void Awake()
        {
            Name = "Натиск";
            Style = "DefaultTactic";
            Damage = 0;
            CardDescription = "Наносит урон выбранной цели";
        }

        public override void Attack(Card target)
        {
            target.GetDamage(Owner.AmplifiedDamage);
        }
    }
}
