using UnityEngine;

namespace Cards.Cyberpunk
{
    public class CyberRat : Card
    {
        protected override void Awake()
        {
            Name = "Кибер крыса";
            Cost = 2;
            Damage = 0;
            MaxHealth = 5;
            IsCardWithActiveSpell = false;
            IsSpecial = true;
            Spell = "Копия";
            CardDescription = "(акт) Перенимает показатель урона у выбранного соседа по сектору";
            CheckUpgrades();
        }
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(79)) IsCardWithActiveSpell = true;
        }

        public override void ActiveSpell(Card target)
        {
            if (target.Sector != Sector || target.Side != Side) return;
            Damage = target.Damage;
            RecalculateDamage();
            Owner.Damage += AmplifiedDamage;
            Owner.RecalculateDamage();
            IsCardWithActiveSpell = false;
        }
    }
}
