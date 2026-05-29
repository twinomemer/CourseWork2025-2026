using UnityEngine;

namespace Cards.Cyberpunk
{
    public class FieldTechnician : Card
    {
        protected override void Awake()
        {
            Name = "Полевой техник";
            Tech = "Cyber";
            Cost = 3;
            Damage = 0;
            MaxHealth = 15;
            IsCardWithActiveSpell = true;
            IsSpecial = true;
            Spell = "Полевой ремонт";
            SpellType = "TargetAlly";
            CardDescription = "(акт) Восстанавливает 5 здоровья любой союзной карте раз в ход";
            CheckUpgrades();
        }

        public override void ActiveSpell(Card target)
        {
            if (target.Side != Side || target.isWounded) return;
            target.Heal(5);
            IsCardWithActiveSpell = false;
        }

        public override void HandleUpdate()
        {
            base.HandleUpdate();
            IsCardWithActiveSpell = true;
        }
    }
}
