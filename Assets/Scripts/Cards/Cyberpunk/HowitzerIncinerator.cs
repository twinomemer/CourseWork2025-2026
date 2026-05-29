namespace Cards.Cyberpunk
{
    public class HowitzerIncinerator : Card
    {
        protected override void Awake()
        {
            Name = "Гаубица 'Испепелитель'";
            Tech = "Cyber";
            Cost = 4;
            Damage = 0;
            MaxHealth = 5;
            IsSpecial = true;
            IsCardWithActiveSpell = true;
            Spell = "Огонь!";
            SpellType = "TargetEnemy";
            CardDescription = "(акт) Наносит 15 урона выбранной доступной для атаки карте раз в ход";
            CheckUpgrades();
        }

        public override void ActiveSpell(Card target)
        {
            if (!target.IsVulnerable) return;
            target.GetDamage(15);
            IsCardWithActiveSpell = false;
        }

        public override void HandleUpdate()
        {
            base.HandleUpdate();
            IsCardWithActiveSpell = true;
        }
    }
}
