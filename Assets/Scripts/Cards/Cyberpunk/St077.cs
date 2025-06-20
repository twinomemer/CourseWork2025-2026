namespace Cards.Cyberpunk
{
    public class St077 : Card
    {
        protected override void Awake()
        {
            Name = "СТ-077";
            Cost = 4;
            Damage = 5;
            MaxHealth = 20;
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(54)) MaxHealth += 10;
            if (IntersceneData.Instance.PlayerUpgrades.Contains(102)) Damage += 5;
        }
    }
}
