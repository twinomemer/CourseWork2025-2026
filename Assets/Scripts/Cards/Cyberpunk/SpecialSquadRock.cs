namespace Cards.Cyberpunk
{
    public class SpecialSquadRock : Card
    {
        private int _healValue = 3;
        private bool _isUpgraded = false;

        protected override void Awake()
        {
            Name = "Спецотряд 'Скала'";
            Cost = 3;
            Damage = 0;
            MaxHealth = 25;
            IsSpecial = true;
            Spell = "Регенерация";
            CardDescription = "(пас) Восстанавливает 3 здоровья в конце хода";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(108)) _isUpgraded = true;
        }

        public override void HandleUpdate()
        {
            if (_isUpgraded) Heal(_healValue);
            base.HandleUpdate();
        }
    }
}
