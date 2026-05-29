using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cards.Biopunk
{
    public class LivingShield : Card
    {
        private int _resistBuff = 1;
        private List<Card> _buffedCards = new List<Card>();
        
        protected override void Awake()
        {
            Name = "Живой щит";
            Tech = "Bio";
            Cost = 4;
            Damage = 0;
            MaxHealth = 30;
            IsSpecial = true;
            Spell = "Защита";
            CardDescription = "(пас) Уменьшает получаемый всеми союзными картами урон на 1";
            CheckUpgrades();
        }

        public override void Initialize(int targetSector)
        {
            HandleUpdate();
            base.Initialize(targetSector);
        }
        private void Buff(Card target)
        {
            target.IncomingDamageReduction += _resistBuff;
            _buffedCards.Add(target);
        }
   
        public override void HandleUpdate()
        {
            if (!isWounded)
            {
                _buffedCards.RemoveAll(obj => obj == null);
                if (!_buffedCards.Contains(this)) Buff(this);
                var cards = FindObjectsOfType<Card>();
                foreach (var card in cards)
                {
                    if (card.IsActive && !_buffedCards.Contains(card) && card.Side == Side && !card.isWounded)
                    {
                        Buff(card);
                    }
                }
            }
            
            base.HandleUpdate();
        }
        
        protected override IEnumerator Die()
        {
            foreach (var card in _buffedCards.ToList())
            {
                card.IncomingDamageReduction -= _resistBuff;
                _buffedCards.Remove(card);
            }
            
            yield return base.Die();
        }
    }
}