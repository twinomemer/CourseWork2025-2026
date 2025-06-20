namespace Cards.Cyberpunk
{
    public class Claw : Card
    {
        protected override void Awake()
        {
            Name = "Клешня";
            Cost = 2;
            Damage = 2;
            MaxHealth = 14;
            IsSpecial = true;
            Spell = "Щёлк!";
            CardDescription = "(акт) Наносит 5 урона выбранной доступной для атаки карте";
            CheckUpgrades();
        }
        protected override void CheckUpgrades()
        {
            IsCardWithActiveSpell = IntersceneData.Instance.PlayerUpgrades.Contains(35);
        }

        public override void ActiveSpell(Card target)
        {
            if (!target.IsVulnerable) return;
            target.GetDamage(5);
            IsCardWithActiveSpell = false;
        }
    }
}
