using Random = UnityEngine.Random;
namespace Cards.Biopunk
{
    public class FailedExperiment : Card
    {
        private int _missChance = 0;

        protected override void Awake()
        {
            Name = "Неудачный эксперимент";
            Cost = 1;
            Damage = 0;
            MaxHealth = 7;
            IsSpecial = true;
            Spell = "Уклонение";
            CardDescription = "(пас) Имеет 10% шанс избежать урона";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(129)) _missChance = 10;
        }
        
        public override void GetDamage(int damage)
        {
            var n = Random.Range(1, 101);
            if (n <= _missChance) return;
            base.GetDamage(damage);
        }
    }
}