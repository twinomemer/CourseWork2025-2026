using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Cards.Cyberpunk
{
    public class SpecialSquadRock : Card
    {
        private int _healValue = 5, _damageValue = 5;
        private bool _isUpgraded = false;

        protected override void Awake()
        {
            Name = "Спецотряд 'Скала'";
            Tech = "Cyber";
            Cost = 3;
            Damage = 0;
            MaxHealth = 25;
            IsSpecial = true;
            Spell = "Регенерация";
            CardDescription = "(пас) Восстанавливает 5 здоровья в конце хода. Когда подвергается атаке, наносит 5 урона случайной карте противника.";
            CheckUpgrades();
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(108)) _isUpgraded = true;
        }

        public override void GetDamage(int damage)
        {
            var cards = FindObjectsOfType<Card>().Where(card => card.IsActive && card.Side != Side).ToArray();
            var targetNum = Random.Range(0, cards.Length);
            cards[targetNum].GetDamage(_damageValue);
            base.GetDamage(damage);
        }

        public override void HandleUpdate()
        {
            if (_isUpgraded && !isWounded) Heal(_healValue);
            base.HandleUpdate();
        }
    }
}
