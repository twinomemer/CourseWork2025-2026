using System.Collections;
using System.Collections.Generic;

namespace Cards.Cyberpunk
{
    public class ConstructionTeam : Card
    {
        protected override void Awake()
        {
            Name = "Стройотряд";
            Cost = 4;
            Damage = 0;
            MaxHealth = 20;
            IsSpecial = true;
            Spell = "Укрепление";
            CardDescription = "(пас) Уменьшает получаемый картами в своём секторе урон на 5";
            CheckUpgrades();
        }
        
        private int _resistBuff = 5;
        private List<Card> _buffedCards = new List<Card>();
        private bool _isUpgraded = false;
        
        public override void Initialize(int targetSector)
        {
            if (_isUpgraded) HandleUpdate();
            base.Initialize(targetSector);
        }
        private void Buff(Card target)
        {
            target.IncomingDamageReduction += _resistBuff;
            _buffedCards.Add(target);
        }
        
        protected override void CheckUpgrades()
        {
            if (IntersceneData.Instance.PlayerUpgrades.Contains(71) 
                && IntersceneData.Instance.PlayerUpgrades.Contains(73) 
                && IntersceneData.Instance.PlayerUpgrades.Contains(81)) _isUpgraded = true;
        }

        public override void HandleUpdate()
        {
            if (_isUpgraded)
            {
                _buffedCards.RemoveAll(obj => obj == null);
                if (!_buffedCards.Contains(this)) Buff(this);
                var cards = FindObjectsOfType<Card>();
                foreach (var card in cards)
                {
                    if (card.IsActive && !_buffedCards.Contains(card) && card.Side == Side && card.Sector == Sector)
                    {
                        Buff(card);
                    }
                }
            }
            base.HandleUpdate();
        }
        
        protected override IEnumerator Die()
        {
            if (_isUpgraded)
            {
                foreach (var card in _buffedCards)
                {
                    card.IncomingDamageReduction -= _resistBuff;
                }
            }
            yield return base.Die();
        }
    }
}
