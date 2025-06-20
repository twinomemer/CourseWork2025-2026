using BattleScene;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Cards.Cyberpunk.Heroes
{
    public class Human001 : Hero
    {
        protected override void Awake()
        {
            Name = "Человек 001";
            Damage = 3;
            MaxHealth = 100;
            IsActive = true;
            Side = 1;
            Balance = 5;
            Health = MaxHealth;
            IsSpecial = true;
            Spell = "Критический урон";
            CardDescription = "(пас) Имеет 17% шанс нанести дополнительные 30% урона при атаке";
        }
        
        public override void Attack(Card target)
        {
            var n = Random.Range(1, 100);
            if (n <= 17)
            {
                var baseDamage = AmplifiedDamage;
                AmplifiedDamage = (int)(AmplifiedDamage*1.3);
                base.Attack(target);
                AmplifiedDamage = baseDamage;
            }
            else base.Attack(target);
        }
    }
}
