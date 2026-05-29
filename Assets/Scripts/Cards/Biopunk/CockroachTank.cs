namespace Cards.Biopunk
{
    public class CockroachTank : Card
    {
        private int _healValue = 7;
        private bool _isUpgraded = false;

        protected override void Awake()
        {
            Name = "Таракан-танк";
            Tech = "Bio";
            Cost = 3;
            Damage = 0;
            MaxHealth = 15;
            IncomingDamageReduction = 7;
            IsSpecial = true;
            Spell = "Живучесть";
            CardDescription = "(пас) Уменьшает входящий урон на 7 и восстанавливает 7 здоровья в конце хода";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(169)) _isUpgraded = true;
        }

        public override void HandleUpdate()
        {
            if (_isUpgraded && !isWounded) Heal(_healValue);
            base.HandleUpdate();
        }
    }
}