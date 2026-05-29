namespace Cards.Cyberpunk
{
    public class SniperGroup : Card
    {
        private bool _isUpgraded = false;

        protected override void Awake()
        {
            Name = "Снайперская группа";
            Tech = "Cyber";
            Cost = 3;
            Damage = 10;
            MaxHealth = 5;
            IsSpecial = true;
            Spell = "Маскировка";
            CardDescription = "(пас) Полностью избегает урона 1 раз";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(72)) _isUpgraded = true;
            if (IntersceneData.Instance.PlayerUpgrades.Contains(93)) Damage += 5;
        }

        public override void GetDamage(int damage)
        {
            if (SpellUsageCount == 0 && _isUpgraded)
            {
                SpellUsageCount += 1;
                OnCardStateChanged?.Invoke($"Карта {Name} избежала урона");
                return;
            }
            base.GetDamage(damage);
        }
    }
}
