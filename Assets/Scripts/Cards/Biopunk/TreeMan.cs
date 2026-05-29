namespace Cards.Biopunk
{
    public class TreeMan : Card
    {
        protected override void Awake()
        {
            Name = "Человек-дерево";
            Tech = "Bio";
            Cost = 2;
            Damage = 5;
            MaxHealth = 10;
            CheckUpgrades();
        }

        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(152)) MaxHealth += 5;
        }
    }
}