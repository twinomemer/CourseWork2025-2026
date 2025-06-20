namespace Cards.Cyberpunk
{
    public class SniperGroup : Card
    {
        private bool _isUpgraded = false;

        protected override void Awake()
        {
            Name = "Снайперская группа";
            Cost = 3;
            Damage = 5;
            MaxHealth = 10;
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
                return;
            }
            base.GetDamage(damage);
        }
    }
}
