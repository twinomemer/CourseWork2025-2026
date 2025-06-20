namespace Cards.Biopunk
{
    public class CockroachTank : Card
    {
        private int _healValue = 7;
        private bool _isUpgraded = false;

        protected override void Awake()
        {
            Name = "Таракан-танк";
            Cost = 3;
            Damage = 0;
            MaxHealth = 15;
            IsSpecial = true;
            Spell = "Регенерация";
            CardDescription = "(пас) Восстанавливает 7 здоровья в конце хода";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(169)) _isUpgraded = true;
        }

        public override void HandleUpdate()
        {
            if (_isUpgraded) Heal(_healValue);
            base.HandleUpdate();
        }
    }
}