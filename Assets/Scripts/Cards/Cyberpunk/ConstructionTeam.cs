using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cards.Cyberpunk
{
    public class ConstructionTeam : Card
    {
        private int _resistBuff = 5;
        private List<Card> _buffedCards = new List<Card>();
        private bool _isUpgraded = false;
        
        protected override void Awake()
        {
            Name = "Стройотряд";
            Tech = "Cyber";
            Cost = 4;
            Damage = 0;
            MaxHealth = 25;
            IncomingDamageReduction = 5;
            IsSpecial = true;
            Spell = "Укрепление";
            CardDescription = "(пас) Уменьшает получаемый картами в своём секторе урон на 5, а для себя на 10";
            CheckUpgrades();
        }
        
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
            if (_isUpgraded && !isWounded)
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
                foreach (var card in _buffedCards.ToList())
                {
                    card.IncomingDamageReduction -= _resistBuff;
                    _buffedCards.Remove(card);
                }
            }
            yield return base.Die();
        }
    }
}
