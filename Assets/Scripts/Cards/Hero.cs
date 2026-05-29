using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Cards
{
    public abstract class Hero : Card
    {
        public int Balance { get; set; }
        public int MaxBalance { get; set; } = 5;
        
        public decimal SpellPower { get; set; }
        
        
        public UnityAction DamageTaken;
        public UnityAction<string> OnCardStateChanged;

        public TMP_Text balanceText;

        protected override void Awake()
        {
            if (randSounds == null) randSounds = new List<SoundArrays>();
        }
        
        protected override void Start()
        {
            DestroyingSoundsSource = GameObject.FindWithTag("DSSource").GetComponent<AudioSource>();
            AudioSource = GetComponent<AudioSource>();
            if (AudioSource == null)
            {
                gameObject.AddComponent<AudioSource>();
                AudioSource = GetComponent<AudioSource>();
            }
            
            InfoScreen = transform.Find("InfoScreen")?.gameObject;
            if (InfoScreen != null) InfoScreen.SetActive(true);
            RecalculateDamage();
            Health = MaxHealth;

            if (transform.Find("HeroImage"))
            {
                CardImage = transform.Find("HeroImage").GetComponent<Image>();
                CardImageInfo = transform.Find("InfoScreen/HeroImageInfo").GetComponent<Image>();
                
                Sprite image = null;
                image = Resources.Load<Sprite>($"Pictures/HeroImages/{Name}");
            
                CardImage.sprite = image;
                CardImageInfo.sprite = image;
            }
            
            
            NameText = transform.Find("HeroName")?.GetComponent<TMP_Text>();
            DamageText = transform.Find("HeroDamage")?.GetComponent<TMP_Text>();
            HealthText = transform.Find("HeroHealth")?.GetComponent<TMP_Text>();
            CardStats = transform.Find("InfoScreen/HeroStats")?.GetComponent<TMP_Text>();
            if (CardStats != null) CardStats.text = $"{Name}\nЗдоровье: {MaxHealth} | Урон: {Damage}";
            if (IsSpecial)
            {
                SpellDescription = transform.Find("InfoScreen/HeroDescription")?.GetComponent<TMP_Text>();
                if (SpellDescription != null) SpellDescription.text = CardDescription;
                
                SpellText = transform.Find("HeroSpell")?.GetComponent<TMP_Text>();
                if (SpellText != null) SpellText.text = Spell;
            }
            statusImage = transform.Find("StatusImage").GetComponent<Image>();
            
            Sprite frameImage = null;
            if (GetComponentInParent<Hero>().Tech == "Bio")
            {
                frameImage = Resources.Load<Sprite>("Pictures/Frames/bio_frame1");
            }
            else if (GetComponentInParent<Hero>().Tech == "Cyber")
            {
                frameImage = Resources.Load<Sprite>("Pictures/Frames/cyber_frame1");
            }
            
            if (transform.Find("Frames"))
            {
                foreach (var frame in transform.Find("Frames").GetComponentsInChildren<Image>())
                {
                    frame.sprite = frameImage;
                }
                foreach (var frame in transform.Find("InfoScreen/FramesInfo").GetComponentsInChildren<Image>())
                {
                    frame.sprite = frameImage;
                }
            }
            
            if (InfoScreen != null)InfoScreen.SetActive(false);
            UpdateCardDisplay();
        }

        protected void OnEnable()
        {
            if (Name == IntersceneData.Instance.Player1?.Name) Side = 1;
            else if (Name == IntersceneData.Instance.Player2?.Name) Side = 2;
        }

        public override void GetDamage(int damage)
        {
            Health -= (damage - IncomingDamageReduction);
            DamageTaken?.Invoke();
            OnCardStateChanged?.Invoke($"Герой {Name} получил {damage - IncomingDamageReduction} урона");
            UpdateCardDisplay();
        }
        
        public override void HandleUpdate()
        {
            RecalculateDamage();
            UpdateCardDisplay();
        }

        public override void UpdateCardDisplay()
        {
            if (balanceText != null) balanceText.text = Balance.ToString();
            base.UpdateCardDisplay();
        }
        
        public virtual void Attack(Card target)
        {
            var tacticCard = FindObjectsOfType<TacticCard>().FirstOrDefault(h => h.Side == Side && h.IsActive);
            tacticCard?.Attack(target);
        }
    }
}
