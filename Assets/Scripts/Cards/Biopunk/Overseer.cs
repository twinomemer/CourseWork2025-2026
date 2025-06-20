using System.Collections;

namespace Cards.Biopunk
{
    public class Overseer : Card
    {
        private bool _isUpgraded = false, _isBuffed;

        protected override void Awake()
        {
            Name = "Наблюдатель";
            Cost = 2;
            Damage = 3;
            MaxHealth = 10;
            IsSpecial = true;
            Spell = "Как посмотреть";
            CardDescription = "(пас) Увеличивает урон союзного героя на 10%";
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
            _isBuffed = false;
            base.Initialize(sector);
        }

        public override void HandleUpdate()
        {
            base.HandleUpdate();
            if (!_isUpgraded || _isBuffed) return;
            Owner.DamageAmplifier += 0.1f;
            Owner.RecalculateDamage();
            _isBuffed = true;
        }

        protected override IEnumerator Die()
        {
            Owner.DamageAmplifier -= 0.1f;
            Owner.RecalculateDamage();
            yield return base.Die();
        }
    }
}