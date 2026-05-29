namespace Cards.Biopunk
{
    public class Rotsnake : Card
    {
        private int _charges;
        protected override void Awake()
        {
            Name = "Гнилозмей";
            Tech = "Bio";
            Cost = 5;
            Damage = 10;
            MaxHealth = 35;
            IsCardWithActiveSpell = true;
            IsSpecial = true;
            Spell = "Залп";
            SpellType = "TargetEnemy";
            CardDescription = "(акт) Может трижды нанести 5 урона выбранной доступной карте противника";
            CheckUpgrades();
        }

        public override void Initialize(int targetSector)
        {
            _charges = 3;
            base.Initialize(targetSector);
        }

        public override void ActiveSpell(Card target)
        {
            if (!target.IsVulnerable) return;
            target.GetDamage(5);
            _charges--;
            if (_charges <= 0) IsCardWithActiveSpell = false;
        }
    }
}