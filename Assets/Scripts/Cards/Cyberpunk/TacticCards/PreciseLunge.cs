using UnityEngine;

namespace Cards.Cyberpunk.TacticCards
{
    public class PreciseLunge : TacticCard
    {
        
        private void Awake()
        {
            Name = "Точный выпад";
            Damage = 1;
            CardDescription = "Наносит урон выбранной цели";
        }

        public override void Attack(Card target)
        {
            target.GetDamage(Owner.AmplifiedDamage);
        }
    }
}
