namespace Cards.Biopunk
{
    public class Tentacle : Card
    {
        protected override void Awake()
        {
            Name = "Щупальце";
            Tech = "Bio";
            Cost = 1;
            Damage = 4;
            MaxHealth = 6;
            CheckUpgrades();
        }
    }
}