namespace Cards.Biopunk
{
    public class ShieldedMushroom : Card
    {
        protected override void Awake()
        {
            Name = "Боровичок броневичок";
            Tech = "Bio";
            Cost = 2;
            Damage = 7;
            MaxHealth = 1;
            IncomingDamageReduction = 12;
            IsSpecial = true;
            Spell = "Моя шляпка";
            CardDescription = "(пас) Входящий урон уменьшен на 12";
            CheckUpgrades();
        }
    }
}