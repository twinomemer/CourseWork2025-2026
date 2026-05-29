using UnityEngine;

namespace Cards.Cyberpunk
{
    public class Defibrillator : Card
    {
        protected override void Awake()
        {
            Name = "Защитный дрон";
            Tech = "Cyber";
            Cost = 2;
            Damage = 5;
            MaxHealth = 10;
            IsCardWithActiveSpell = true;
            IsSpecial = true;
            Spell = "Защитное поле";
            SpellType = "TargetAlly";
            CardDescription = "(акт) Навсегда увеличивает блок урона на 2 для любой союзной карты";
            CheckUpgrades();
        }

        public override void ActiveSpell(Card target)
        {
            if (target.Side != Side || target.isWounded) return;
            target.IncomingDamageReduction += 2;
            IsCardWithActiveSpell = false;
        }
    }
}
