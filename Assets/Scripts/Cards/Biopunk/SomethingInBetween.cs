namespace Cards.Biopunk
{
    public class SomethingInBetween : Card
    {
        protected override void Awake()
        {
            Name = "Нечто среднее";
            Tech = "Bio";
            Cost = 1;
            Damage = 5;
            MaxHealth = 5;
            CheckUpgrades();
        }
    }
}