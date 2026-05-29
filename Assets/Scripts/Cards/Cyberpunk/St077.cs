namespace Cards.Cyberpunk
{
    public class St077 : Card
    {
        protected override void Awake()
        {
            Name = "СТ-077";
            Tech = "Cyber";
            Cost = 4;
            Damage = 10;
            MaxHealth = 15;
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(54)) MaxHealth += 10;
            if (IntersceneData.Instance.PlayerUpgrades.Contains(102)) Damage += 5;
        }
    }
}
