namespace Cards.Cyberpunk
{
    public class CyberRabble : Card
    {
        protected override void Awake()
        {
            Name = "Кибер чернь";
            Cost = 1;
            Damage = 1;
            MaxHealth = 6;
            CheckUpgrades();
        }
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(16)) Damage += 3;
        }
    }
}
