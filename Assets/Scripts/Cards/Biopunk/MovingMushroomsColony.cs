namespace Cards.Biopunk
{
    public class MovingMushroomsColony : Card
    {
        protected override void Awake()
        {
            Name = "Двигающиеся грибы";
            Cost = 1;
            Damage = 0;
            MaxHealth = 7;
            IsCardWithActiveSpell = false;
            IsSpecial = true;
            Spell = "Бурные споры";
            CardDescription = "(акт) Наносят 2 урона всем вражеским картам в выбранном доступном секторе";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(168)) IsCardWithActiveSpell = true;
        }
        
        public override void ActiveSpell(Card target)
        {
            if (!target.IsVulnerable) return;
            var cards = FindObjectsOfType<Card>();
            foreach (var card in cards)
            {
                if (card.IsActive && card.Side == target.Side && card.Sector == target.Sector)
                {
                    card.GetDamage(2);
                }
            }
            IsCardWithActiveSpell = false;
        }
    }
}