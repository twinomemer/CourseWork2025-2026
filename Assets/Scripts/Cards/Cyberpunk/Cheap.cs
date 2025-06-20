namespace Cards.Cyberpunk
{
    public class Cheap : Card
    {
        protected override void Awake()
        {
            Name = "Дешёвка";
            Cost = 1;
            Damage = 3;
            MaxHealth = 5;
            CheckUpgrades();
        }
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(14)) MaxHealth += 2;
        }
    }
}
