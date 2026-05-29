namespace Cards.Biopunk
{
    public class Hedge : Card
    {
        protected override void Awake()
        {
            Name = "Живая изгородь";
            Tech = "Bio";
            Cost = 2;
            Damage = 0;
            MaxHealth = 10;
            IsSpecial = true;
            Spell = "Живучесть";
            CardDescription = "(пас) Полностью избегает урона 1 раз";
            CheckUpgrades();
        }

        public override void GetDamage(int damage)
        {
            if (SpellUsageCount == 0)
            {
                SpellUsageCount += 1;
                OnCardStateChanged?.Invoke($"Карта {Name} избежала урона");
                return;
            }
            base.GetDamage(damage);
        }
    }
}