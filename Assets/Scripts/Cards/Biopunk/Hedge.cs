namespace Cards.Biopunk
{
    public class Hedge : Card
    {
        protected override void Awake()
        {
            Name = "Живая изгородь";
            Cost = 2;
            Damage = 0;
            MaxHealth = 10;
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(170)) MaxHealth += 10;
        }
    }
}