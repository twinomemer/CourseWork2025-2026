namespace Cards.Cyberpunk
{
    public class TeslaTower : Card
    {
        protected override void Awake()
        {
            Name = "Башня Теслы";
            Tech = "Cyber";
            Cost = 4;
            Damage = 25;
            MaxHealth = 15;
            CheckUpgrades();
        }
    }
}
