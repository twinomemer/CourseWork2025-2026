namespace Cards.Cyberpunk
{
    public class AssaultUnit : Card
    {
        protected override void Awake()
        {
            Name = "Штурмовое подразделение";
            Cost = 3;
            Damage = 3;
            MaxHealth = 20;
            CheckUpgrades();
        }
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(92)) Damage += 3;
            if (IntersceneData.Instance.PlayerUpgrades.Contains(94)) Damage += 4;
        }
    }
}
