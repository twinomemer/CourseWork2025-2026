using System;
using System.Collections;
using System.Collections.Generic;
using Sounds;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Cards
{
    public abstract class Card : SoundManager, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public string Name { get; protected set; }
        public int Cost { get; set; }
        public int Damage { get; set; }
        public int AmplifiedDamage { get; set; }
        public int MaxHealth { get; protected set; }
        public int Health { get; protected set; }
        public decimal DamageAmplifier { get; set; } = 1;
        public int IncomingDamageReduction { get; set; } = 0;
        public int SpellUsageCount { get; protected set; }
        public int Sector { get; set; }
        public int Side { get; protected set; }
        public bool IsActive { get; protected set; }
        public bool IsVulnerable { get; set; }
        public bool IsCardWithActiveSpell { get; set; } = false;
        protected bool IsSpecial { get; set; } = false;
        protected string Spell { get; set; }
        public string CardDescription { get; set; }
        public string SpellType { get; set; }
        public string Tech { get; protected set; }
        public Hero Owner { get; set; }
        
        protected TMP_Text NameText;
        protected TMP_Text CostText;
        protected TMP_Text DamageText;
        protected TMP_Text HealthText;
        protected TMP_Text SpellText;
        protected TMP_Text SpellDescription;
        protected TMP_Text CardStats;
        protected TMP_Text CardTimer;
        protected Image CardImage;
        protected Image CardImageInfo;
        protected GameObject InfoScreen;
        protected bool IsRightButtonHeld;
        protected RectTransform WholeCardRect;
        protected float AnimationDuration = 0.7f;
        protected Vector2 savedCursorPosition;
        public int turnsWounded;
        public bool isWounded = false;
        public Image statusImage;

        public UnityAction OnCardInitialized, OnCardWounded, OnCardDead;
        public UnityAction<Card> OnCardClicked;
        public UnityAction<string> OnCardStateChanged;

        protected abstract void Awake();
        
        protected override void Start()
        {
            base.Start();

            if (randSounds == null) randSounds = new List<SoundArrays>();
            var soundArrays = new SoundArrays();
            soundArrays.soundArray = new List<AudioClip>();
            for (var i = 0; i < 4; i++)
            {
                var fullPath = $"SoundEffects/card_played{i + 1}";
                var sound = Resources.Load<AudioClip>(fullPath);
                if (sound != null)
                    soundArrays.soundArray.Add(sound);
                else
                    Debug.LogError($"Звук не найден: {fullPath}");
            }
            randSounds.Add(soundArrays);
            
            InfoScreen = transform.Find("InfoScreen")?.gameObject;
            if (InfoScreen != null) InfoScreen.SetActive(true);
            RecalculateDamage();
            Health = MaxHealth;
            WholeCardRect = GetComponent<RectTransform>();

            if (transform.Find("CardImage"))
            {
                CardImage = transform.Find("CardImage").GetComponent<Image>();
                CardImageInfo = transform.Find("InfoScreen/CardImageInfo").GetComponent<Image>();
                
                Sprite image = null;
                image = Resources.Load<Sprite>($"Pictures/CardImages/{Name}");
            
                CardImage.sprite = image;
                CardImageInfo.sprite = image;
            }
            
            
            NameText = transform.Find("CardName")?.GetComponent<TMP_Text>();
            CostText = transform.Find("CardPrice")?.GetComponent<TMP_Text>();
            DamageText = transform.Find("CardDamage")?.GetComponent<TMP_Text>();
            HealthText = transform.Find("CardHealth")?.GetComponent<TMP_Text>();
            CardTimer = transform.Find("CardTimer")?.GetComponent<TMP_Text>();
            CardStats = transform.Find("InfoScreen/CardStats")?.GetComponent<TMP_Text>();
            if (CardStats != null) CardStats.text = $"{Name} ({Cost})\nЗдоровье: {MaxHealth} | Урон: {Damage}";
            if (IsSpecial)
            {
                SpellDescription = transform.Find("InfoScreen/CardDescription")?.GetComponent<TMP_Text>();
                if (SpellDescription != null) SpellDescription.text = CardDescription;
                
                SpellText = transform.Find("CardSpell")?.GetComponent<TMP_Text>();
                if (SpellText != null) SpellText.text = Spell;
            }
            statusImage = transform.Find("StatusImage").GetComponent<Image>();

            Sprite frameImage = null;
            if (GetComponentInParent<Card>().Tech == "Bio")
            {
                frameImage = Resources.Load<Sprite>("Pictures/Frames/bio_frame1");
            }
            else if (GetComponentInParent<Card>().Tech == "Cyber")
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

        public virtual void Initialize(int targetSector)
        {
            PlaySound(0, true);
            turnsWounded = Cost switch
            {
                1 or 2 or 3 or 4 => 3,
                5 => 4,
                _ => turnsWounded
            };

            isWounded = false;
            StartCoroutine(AnimateCard(Vector3.zero, new Vector3(0.85f, 0.9f, 0.9f)));
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (Owner?.Name == "Жучий маршал") RescaleHealth((int) (MaxHealth * Owner.SpellPower));
            IsActive = true;
            SpellUsageCount = 0;
            Health = MaxHealth;
            Side = Owner.Side;
            Owner.Balance -= Cost;
            RecalculateDamage();
            Owner.Damage += AmplifiedDamage;
            Owner.RecalculateDamage();
            Sector = targetSector;
            OnCardInitialized?.Invoke();
        }
        
        public virtual void GetDamage(int damage)
        {
            var incomingDamage = damage - IncomingDamageReduction;
            if (incomingDamage < 0) incomingDamage = 0;
            Health -= incomingDamage;
            OnCardStateChanged?.Invoke($"Карта {Name} получила {incomingDamage} урона");
            if (Health <= 0 && !isWounded) StartCoroutine(Die());
            else UpdateCardDisplay();
        }

        public void RescaleHealth(int newMaxHealth)
        {
            
            var difference = newMaxHealth - MaxHealth;
            MaxHealth = newMaxHealth;
            OnCardStateChanged?.Invoke($"Максимальное здоровье карты {Name} изменено на {difference}");
            Heal(difference);
            UpdateCardDisplay();
        }
        public void RecalculateDamage()
        {
            AmplifiedDamage = (int) Math.Truncate(Damage * DamageAmplifier);
            UpdateCardDisplay();
        }

        public void Heal(int amount)
        {
            if (Health == MaxHealth) return;
            OnCardStateChanged?.Invoke(Health + amount > MaxHealth
                ? $"Карта {Name} восстановила {MaxHealth - Health} здоровья"
                : $"Карта {Name} восстановила {amount} здоровья");
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
            UpdateCardDisplay();
            if (Health <= 0 && !isWounded) StartCoroutine(Die());
        }

        protected virtual IEnumerator Die()
        {
            if (!IsActive) yield break;
            OnCardStateChanged?.Invoke($"Карта {Name} уничтожена");
            IsActive = false;
            Owner.Damage -= AmplifiedDamage;
            Owner.RecalculateDamage();
            Awake();
            Health = MaxHealth;
            RecalculateDamage();
            statusImage.color = new Color(0.5f,0.5f, 0.5f, 0.7f);
            isWounded = true;
            OnCardWounded?.Invoke();
            StartCoroutine(BeingWounded());
        }

        public IEnumerator BeingWounded()
        {
            if (!CardTimer.IsActive()) CardTimer.gameObject.SetActive(true);
            if (CardTimer && isWounded) CardTimer.text = $"{turnsWounded}";
            if (turnsWounded <= 0)
            {
                statusImage.color = new Color(1f, 1f, 1f, 0f);
                isWounded = false;
                yield return StartCoroutine(AnimateCard(Vector3.one, Vector3.zero));
                OnCardDead?.Invoke();
                WholeCardRect.localScale = Vector3.one;
                CardTimer.gameObject.SetActive(false);
            }
        }

        public virtual void UpdateCardDisplay()
        {
            if (NameText) NameText.text = Name;
            if (CostText) CostText.text = Cost.ToString();
            if (DamageText) DamageText.text = AmplifiedDamage.ToString();
            if (HealthText) HealthText.text = Health.ToString();
        }
        
        public virtual void HandleUpdate()
        {
            Owner.Damage -= AmplifiedDamage;
            RecalculateDamage();
            Owner.Damage += AmplifiedDamage;
            Owner.RecalculateDamage();
            if (isWounded)
            {
                turnsWounded -= 1;
                StartCoroutine(BeingWounded());
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnCardClicked?.Invoke(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Right){}
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) return;
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                savedCursorPosition =  eventData.position;
                IsRightButtonHeld = true;
                //Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                StartCoroutine(ShowWindowAfterDelay());
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                IsRightButtonHeld = false;
                //Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                HideWindow();
            }
        }

        private IEnumerator ShowWindowAfterDelay(float delay = 0.1f)
        {
            yield return new WaitForSeconds(delay);
        
            if (IsRightButtonHeld)
            {
                ShowWindow();
                var newTransform = GameObject.FindGameObjectWithTag("AdditionalCanvas").transform; // --------------------------------------------------------------плохо
                InfoScreen.transform.SetParent(newTransform);
            }
        }

        private void ShowWindow()
        {
            if (InfoScreen != null)
            {
                InfoScreen.SetActive(true);
                InfoScreen.transform.localScale = new Vector3(1f, 1f, 1f);
                InfoScreen.transform.position = new Vector3(0, 0, 0);
            }
        }
        
        private void HideWindow()
        {
            if (InfoScreen != null)
            {
                InfoScreen.SetActive(false);
                InfoScreen.transform.SetParent(transform);
            }
        }
        
        private IEnumerator AnimateCard(Vector3 startScale, Vector3 endScale)
        {
            var elapsed = 0f;
            if (WholeCardRect != null) WholeCardRect.localScale = startScale;

            while (elapsed < AnimationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / AnimationDuration);
                if (WholeCardRect != null) WholeCardRect.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }

            if (WholeCardRect != null) WholeCardRect.localScale = endScale;
        }
        
        protected virtual void CheckUpgrades(){}
        
        public virtual void ActiveSpell(Card target = null){}
        }
}
