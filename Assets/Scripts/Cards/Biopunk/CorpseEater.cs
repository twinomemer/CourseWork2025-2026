namespace Cards.Biopunk
{
    public class CorpseEater : Card
    {
        private int _turn, _spellTurn;
        
        protected override void Awake()
        {
            Name = "Трупоед";
            Tech = "Bio";
            Cost = 4;
            Damage = 5;
            MaxHealth = 20;
            IsCardWithActiveSpell = true;
            IsSpecial = true;
            Spell = "Тризна";
            SpellType = "TargetAllyCorpse";
            CardDescription = "(акт) Раз в три хода поедает союзный труп и увеличивает свой урон на 5.";
            CheckUpgrades();
        }

        public override void Initialize(int targetSector)
        {
            _turn = 1;
            base.Initialize(targetSector);
        }

        public override void ActiveSpell(Card target)
        {
            if (target.Side != Side || !target.isWounded) return;
            _spellTurn = _turn;
            target.turnsWounded = 0;
            target.HandleUpdate();
            Damage += 5;
            Owner.Damage -= AmplifiedDamage;
            RecalculateDamage();
            Owner.Damage += AmplifiedDamage;
            Owner.RecalculateDamage();
            IsCardWithActiveSpell = false;
        }

        public override void HandleUpdate()
        {
            _turn += 1;
            if (_turn == _spellTurn + 3) IsCardWithActiveSpell = true;
            base.HandleUpdate();
        }
    }
}