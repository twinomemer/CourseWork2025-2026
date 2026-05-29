namespace Cards.Biopunk
{
    public class KonstantinTheMachineGunner : Card
    {
        protected override void Awake()
        {
            Name = "Пулемётчик Константин";
            Tech = "Bio";
            Cost = 3;
            Damage = 5;
            MaxHealth = 15;
            CheckUpgrades();
        }

        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(181)) Damage += 10;
        }
    }
}