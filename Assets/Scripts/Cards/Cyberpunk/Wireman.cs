namespace Cards.Cyberpunk
{
    public class Wireman : Card
    {
        protected override void Awake()
        {
            Name = "Проводчик";
            Tech = "Cyber";
            Cost = 1;
            Damage = 5;
            MaxHealth = 5;
            CheckUpgrades();
        }
    }
}
