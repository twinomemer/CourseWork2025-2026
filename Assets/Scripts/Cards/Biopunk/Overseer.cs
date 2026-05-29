using System.Collections;

namespace Cards.Biopunk
{
    public class Overseer : Card
    {
        private bool _isUpgraded = false;

        protected override void Awake()
        {
            Name = "Наблюдатель";
            Tech = "Bio";
            Cost = 2;
            Damage = 3;
            MaxHealth = 10;
            IsSpecial = true;
            Spell = "Как посмотреть";
            CardDescription = "(пас) Увеличивает урон союзного героя на 15%";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(154)
                && IntersceneData.Instance.PlayerUpgrades.Contains(155)
                && IntersceneData.Instance.PlayerUpgrades.Contains(156)) _isUpgraded = true;
        }

        public override void Initialize(int sector)
        {
            base.Initialize(sector);
            if (!_isUpgraded) return;
            Owner.DamageAmplifier += (decimal)0.15;
            Owner.RecalculateDamage();
        }
        
        protected override IEnumerator Die()
        {
            Owner.DamageAmplifier -= (decimal)0.15;
            Owner.RecalculateDamage();
            yield return base.Die();
        }
    }
}