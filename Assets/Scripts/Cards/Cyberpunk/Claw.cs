namespace Cards.Cyberpunk
{
    public class Claw : Card
    {
        protected override void Awake()
        {
            Name = "Клешня";
            Tech = "Cyber";
            Cost = 2;
            Damage = 2;
            MaxHealth = 10;
            IsSpecial = true;
            Spell = "Щёлк!";
            SpellType = "TargetEnemy";
            CardDescription = "(акт) Наносит 10 урона выбранной доступной для атаки карте";
            CheckUpgrades();
        }
        protected override void CheckUpgrades()
        {
            IsCardWithActiveSpell = IntersceneData.Instance.PlayerUpgrades.Contains(35);
        }

        public override void ActiveSpell(Card target)
        {
            if (!target.IsVulnerable) return;
            target.GetDamage(10);
            IsCardWithActiveSpell = false;
        }
    }
}
