namespace Cards.Biopunk
{
    public class BiomassSource : Card
    {
        private bool _isUpgraded;

        protected override void Awake()
        {
            Name = "Источник биомассы";
            Cost = 4;
            Damage = 0;
            MaxHealth = 30;
            IsSpecial = true;
            Spell = "Подкормка";
            CardDescription = "(пас) Восстанавливает 5 здоровья соседям по сектору в конце хода";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(180)) _isUpgraded = true;
        }

        public override void HandleUpdate()
        {
            if (_isUpgraded)
            {
                var cards = FindObjectsOfType<Card>();
                foreach (var card in cards)
                {
                    if (card.IsActive && card != this &&card.Side == Side && card.Sector == Sector)
                    {
                        card.Heal(5);
                    }
                }
            }
            base.HandleUpdate();
        }
    }
}