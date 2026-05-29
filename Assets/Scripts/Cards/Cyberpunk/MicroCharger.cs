namespace Cards.Cyberpunk
{
    public class MicroCharger : Card
    {
        protected override void Awake()
        {
            Name = "Разрядник М1";
            Tech = "Cyber";
            Cost = 1;
            Damage = 1;
            MaxHealth = 5;
            IsSpecial = true;
            IsCardWithActiveSpell = true;
            Spell = "Ззз";
            SpellType = "TargetEnemy";
            CardDescription = "(акт) Наносит 1 урона выбранной доступной для атаки карте раз в ход";
            CheckUpgrades();
        }

        public override void ActiveSpell(Card target)
        {
            if (!target.IsVulnerable) return;
            target.GetDamage(1);
            IsCardWithActiveSpell = false;
        }

        public override void HandleUpdate()
        {
            base.HandleUpdate();
            IsCardWithActiveSpell = true;
        }
    }
}
