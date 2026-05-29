using Random = UnityEngine.Random;

namespace Cards.Cyberpunk
{
    public class CyberSamurai : Card
    {

        protected override void Awake()
        {
            Name = "Кибер-самурай";
            Tech = "Cyber";
            Cost = 5;
            Damage = 20;
            MaxHealth = 20;
            IsSpecial = true;
            IncomingDamageReduction = 20;
            Spell = "Парирование";
            CardDescription = "(пас) Входящий урон уменьшен на 20";
            CheckUpgrades();
            /*Есть предложение от Димасика сделать урон равный блоку урона. Чтобы его можно было разгонять Защдроном, Стройотрядом и тыры-пыры*/
        }
    }
}
