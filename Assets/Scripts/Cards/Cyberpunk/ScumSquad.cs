namespace Cards.Cyberpunk
{
    public class ScumSquad : Card
    {
        protected override void Awake()
        {
            Name = "Отряд отребья";
            Cost = 1;
            Damage = 1;
            MaxHealth = 6;
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(12)) MaxHealth += 1;
            if (IntersceneData.Instance.PlayerUpgrades.Contains(13)) MaxHealth += 1;
            if (IntersceneData.Instance.PlayerUpgrades.Contains(14)) MaxHealth += 1;
        }
    }
}
