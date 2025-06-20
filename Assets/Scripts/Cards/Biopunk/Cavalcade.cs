using System.Collections;

namespace Cards.Biopunk
{
    public class Cavalcade : Card
    {
        private bool _hasSecondLife = false, _isResistant = false;

        protected override void Awake()
        {
            Name = "Кавалькада";
            Cost = 5;
            Damage = 10;
            MaxHealth = 15;
            IsSpecial = true;
            Spell = "Реинкарнация\nСтойкость";
            CardDescription = "(пас) Перерождается после смерти 1 раз.\n(пас) Получает не более 5 урона за раз";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(198)) _isResistant = true;
            if (IntersceneData.Instance.PlayerUpgrades.Contains(184)) _hasSecondLife = true;
        }

        public override void GetDamage(int damage)
        {
            if (_isResistant)
            {
                var actualDamage = damage - IncomingDamageReduction;
                if (actualDamage > 5) actualDamage = 5;
                damage = actualDamage;
            }
            base.GetDamage(damage);
        }

        protected override IEnumerator Die()
        {
            if (_hasSecondLife)
            {
                Initialize(Sector);
                Health = MaxHealth;
                _hasSecondLife = false;
                UpdateCardDisplay();
            }
            else yield return base.Die();
        }
    }
}