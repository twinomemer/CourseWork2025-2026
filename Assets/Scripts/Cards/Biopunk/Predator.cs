namespace Cards.Biopunk
{
    public class Predator : Card
    {
        protected override void Awake()
        {
            Name = "Хищник";
            Cost = 3;
            Damage = 10;
            MaxHealth = 20;
            IsCardWithActiveSpell = false;
            IsSpecial = true;
            Spell = "Перекачка";
            CardDescription = "(акт) Снижает своё максимальное здоровье на 10, но увеличивает урон на 10";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(182)) IsCardWithActiveSpell = true;
        }

        public override void ActiveSpell(Card target = null)
        {
            RescaleHealth(MaxHealth - 10);
            Damage += 10;
            SpellUsageCount += 1;
            HandleUpdate();
            IsCardWithActiveSpell = false;
        }
    }
}