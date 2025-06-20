namespace Cards.Biopunk
{
    public class PoisonousPlant : Card
    {
        protected override void Awake()
        {
            Name = "Ядовитое растение";
            Cost = 1;
            Damage = 3;
            MaxHealth = 1;
            CheckUpgrades();
        }

        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(133)) Damage += 3;
            if (IntersceneData.Instance.PlayerUpgrades.Contains(151)) Damage += 3;
        }
    }
}