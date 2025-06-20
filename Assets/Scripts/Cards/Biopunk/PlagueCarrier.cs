namespace Cards.Biopunk
{
    public class PlagueCarrier : Card
    {
        protected override void Awake()
        {
            Name = "Разносчик заразы";
            Cost = 1;
            Damage = 3;
            MaxHealth = 1;
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(132)) MaxHealth += 4;
            if (IntersceneData.Instance.PlayerUpgrades.Contains(150)) Damage += 2;
        }
    }
}