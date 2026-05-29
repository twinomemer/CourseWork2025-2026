namespace Cards.Biopunk
{
    public class ModifiedMan : Card
    {
        protected override void Awake()
        {
            Name = "ГМО человек";
            Tech = "Bio";
            Cost = 2;
            Damage = 5;
            MaxHealth = 5;
            CheckUpgrades();
        }

        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(126)) MaxHealth += 5;
            if (IntersceneData.Instance.PlayerUpgrades.Contains(140)) Damage += 5;
        }
    }
}