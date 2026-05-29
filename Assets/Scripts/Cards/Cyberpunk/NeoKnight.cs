namespace Cards.Cyberpunk
{
    public class NeoKnight : Card
    {
        protected override void Awake()
        {
            Name = "Нейрорыцарь"; // Карта в честь Серёги
            Tech = "Cyber";
            Cost = 2;
            Damage = 5;
            MaxHealth = 10;
            IncomingDamageReduction = 5;
            IsSpecial = true;
            Spell = "Щит";
            CardDescription = "(пас) Входящий урон уменьшен на 5";
            CheckUpgrades();
        }
    }
}
