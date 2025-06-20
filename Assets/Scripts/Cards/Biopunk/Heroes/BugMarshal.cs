using System.Linq;
using UnityEngine;

namespace Cards.Biopunk.Heroes
{
    public class BugMarshal : Hero
    {
        protected override void Awake()
        {
            Name = "Жучиный маршал";
            Damage = 3;
            MaxHealth = 100;
            IsActive = true;
            Side = 2;
            Balance = 5;
            Health = MaxHealth;
            IsSpecial = true;
            Spell = "Дополнительное здоровье";
            CardDescription = "(пас) Увеличивает здоровье всех союзных карт на 10%";
        }
    }
}
