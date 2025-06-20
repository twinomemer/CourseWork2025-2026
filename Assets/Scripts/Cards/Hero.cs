using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Cards
{
    public abstract class Hero : Card
    {
        public int Balance { get; set; }

        public UnityAction DamageTaken;

        protected TMP_Text BalanceText;
        
        protected override void Start()
        {
            InfoScreen = transform.Find("InfoScreen")?.gameObject;
            if (InfoScreen != null) InfoScreen.SetActive(true);
            RecalculateDamage();
            Health = MaxHealth;
            if (Side == 1) BalanceText = GameObject.FindWithTag("CurrentEnergy")?.GetComponent<TMP_Text>();
            NameText = transform.Find("HeroName")?.GetComponent<TMP_Text>();
            DamageText = transform.Find("HeroDamage")?.GetComponent<TMP_Text>();
            HealthText = transform.Find("HeroHealth")?.GetComponent<TMP_Text>();
            CardStats = transform.Find("InfoScreen/HeroStats")?.GetComponent<TMP_Text>();
            if (CardStats != null) CardStats.text = $"{Name}\nЗдоровье: {MaxHealth}\nУрон: {Damage - 1}";
            if (IsSpecial)
            {
                SpellDescription = transform.Find("InfoScreen/HeroDescription")?.GetComponent<TMP_Text>();
                if (SpellDescription != null) SpellDescription.text = CardDescription;
                
                SpellText = transform.Find("HeroSpell")?.GetComponent<TMP_Text>();
                if (SpellText != null) SpellText.text = Spell;
            }
            if (InfoScreen != null)InfoScreen.SetActive(false);
            UpdateCardDisplay();
        }
        
        public override void GetDamage(int damage)
        {
            Health -= (damage - IncomingDamageReduction);
            DamageTaken?.Invoke();
            UpdateCardDisplay();
        }
        
        public override void HandleUpdate()
        {
            Balance = 5;
            RecalculateDamage();
            UpdateCardDisplay();
        }

        public override void UpdateCardDisplay()
        {
            if (BalanceText != null) BalanceText.text = Balance.ToString();
            base.UpdateCardDisplay();
        }
        
        public virtual void Attack(Card target)
        {
            var tacticCard = FindObjectsOfType<TacticCard>().FirstOrDefault(h => h.Side == Side && h.IsActive);
            tacticCard?.Attack(target);
        }
    }
}
