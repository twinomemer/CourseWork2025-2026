namespace Cards.Biopunk
{
    public class NutritionalMass : Card
    {

        protected override void Awake()
        {
            Name = "Питательная масса";
            Tech = "Bio";
            Cost = 2;
            Damage = 0;
            MaxHealth = 3;
            IsCardWithActiveSpell = false;
            IsSpecial = true;
            Spell = "Обед";
            SpellType = "TargetAllyCaster";
            CardDescription = "(акт) Ценой жизни восстанавливает заряд способности выбранной союзной карте. Доступно через ход после розыгрыша.";
            CheckUpgrades();
        }

        public override void Initialize(int targetSector)
        {
            base.Initialize(targetSector);
            IsCardWithActiveSpell = false;
        }

        public override void ActiveSpell(Card target)
        {
            if (target.Side != Side || target.isWounded) return;
            turnsWounded += 1;
            target.IsCardWithActiveSpell = true;
            IsCardWithActiveSpell = false;
            StartCoroutine(Die());
        }

        public override void HandleUpdate()
        {
            if (!isWounded) IsCardWithActiveSpell = true;
            base.HandleUpdate();
        }
    }
}