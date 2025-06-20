namespace Cards.Biopunk
{
    public class KonstantinTheMachineGunner : Card
    {
        protected override void Awake()
        {
            Name = "Пулемётчик Константин";
            Cost = 3;
            Damage = 10;
            MaxHealth = 10;
            CheckUpgrades();
        }

        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(181)) Damage += 10;
        }
    }
}