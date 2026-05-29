namespace Cards.Biopunk
{
    public class RottenDevilsTeam : Card
    {
        protected override void Awake()
        {
            Name = "Кислотные бесята";
            Tech = "Bio";
            Cost = 3;
            Damage = 5;
            MaxHealth = 10;
            IsCardWithActiveSpell = true;
            IsSpecial = true;
            Spell = "Ехидный бросок";
            SpellType = "TargetEnemy";
            CardDescription = "(акт) Раз в ход может нанести 5 урона выбранной доступной карте противника";
            CheckUpgrades();
        }

        public override void ActiveSpell(Card target)
        {
            if (!target.IsVulnerable) return;
            target.GetDamage(5);
            IsCardWithActiveSpell = false;
        }

        public override void HandleUpdate()
        {
            base.HandleUpdate();
            IsCardWithActiveSpell = true;
        }
    }
}