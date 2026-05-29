namespace Cards.Cyberpunk
{
    public class NANO : Card
    {
        protected override void Awake()
        {
            Name = "НАНО";
            Tech = "Cyber";
            Cost = 4;
            Damage = 10;
            MaxHealth = 20;
            IncomingDamageReduction = 10;
            IsSpecial = true;
            Spell = "Наномашины";
            CardDescription = "(пас) Входящий урон уменьшен на 10";
            CheckUpgrades();
        }
    }
}
