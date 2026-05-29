namespace Cards.Biopunk
{
    public class BloodBag : Card
    {

        protected override void Awake()
        {
            Name = "Мешок с кровью";
            Tech = "Bio";
            Cost = 1;
            Damage = 0;
            MaxHealth = 10;
            IsCardWithActiveSpell = false;
            IsSpecial = true;
            Spell = "От сердца оторвать";
            SpellType = "TargetEnemy";
            CardDescription = "(акт) Ценой 5 здоровья наносит 7 урона выбранной доступной карте противника";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(120)) IsCardWithActiveSpell = true;
        }

        public override void ActiveSpell(Card target)
        {
            if (!target.IsVulnerable) return;
            if (Health - 5 <= 0) turnsWounded += 1;
            GetDamage(5);
            target.GetDamage(7);
            IsCardWithActiveSpell = false;
        }
    }
}