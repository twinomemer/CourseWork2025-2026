namespace Cards.Cyberpunk
{
    public class NamelessHacker : Card
    {
        protected override void Awake()
        {
            Name = "Безымянный хакер";
            Cost = 1;
            Damage = 1;
            MaxHealth = 6;
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(43)) Damage += 3;
        }
    }
}
