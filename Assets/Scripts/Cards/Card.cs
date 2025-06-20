using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Cards
{
    public abstract class Card : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public string Name { get; protected set; }
        public int Cost { get; set; }
        public int Damage { get; set; }
        public int AmplifiedDamage { get; set; }
        public int MaxHealth { get; protected set; }
        public int Health { get; protected set; }
        public float DamageAmplifier { get; set; } = 1;
        public int IncomingDamageReduction { get; set; } = 0;
        public int SpellUsageCount { get; protected set; }
        public int Sector { get; set; }
        public int Side { get; protected set; }
        public bool IsActive { get; protected set; }
        public bool IsVulnerable { get; set; }
        public bool IsCardWithActiveSpell { get; set; } = false;
        protected bool IsSpecial { get; set; } = false;
        protected string Spell { get; set; }
        protected string CardDescription { get; set; }
        public Hero Owner { get; set; }
        
        protected TMP_Text NameText;
        protected TMP_Text CostText;
        protected TMP_Text DamageText;
        protected TMP_Text HealthText;
        protected TMP_Text SpellText;
        protected TMP_Text SpellDescription;
        protected TMP_Text CardStats;
        protected GameObject InfoScreen;
        protected bool IsRightButtonHeld;
        protected RectTransform WholeCardImage;
        protected float AnimationDuration = 0.7f;

        public UnityAction OnCardInitialized, OnCardDead;
        public UnityAction<Card> OnCardClicked;

        protected abstract void Awake();
        
        protected virtual void Start()
        {
            InfoScreen = transform.Find("InfoScreen")?.gameObject;
            if (InfoScreen != null) InfoScreen.SetActive(true);
            RecalculateDamage();
            Health = MaxHealth;
            WholeCardImage = GetComponent<RectTransform>();
            NameText = transform.Find("CardName")?.GetComponent<TMP_Text>();
            CostText = transform.Find("CardPrice")?.GetComponent<TMP_Text>();
            DamageText = transform.Find("CardDamage")?.GetComponent<TMP_Text>();
            HealthText = transform.Find("CardHealth")?.GetComponent<TMP_Text>();
            CardStats = transform.Find("InfoScreen/CardStats")?.GetComponent<TMP_Text>();
            if (CardStats != null) CardStats.text = $"{Name}\nЦена: {Cost}\nЗдоровье: {MaxHealth}\nУрон: {Damage}";
            if (IsSpecial)
            {
                SpellDescription = transform.Find("InfoScreen/CardDescription")?.GetComponent<TMP_Text>();
                if (SpellDescription != null) SpellDescription.text = CardDescription;
                
                SpellText = transform.Find("CardSpell")?.GetComponent<TMP_Text>();
                if (SpellText != null) SpellText.text = Spell;
            }
            if (InfoScreen != null)InfoScreen.SetActive(false);
            UpdateCardDisplay();
        }

        public virtual void Initialize(int targetSector)
        {
            StartCoroutine(AnimateCard(Vector3.zero, Vector3.one));
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            IsActive = true;
            SpellUsageCount = 0;
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
            Health -= (damage - IncomingDamageReduction);
            if (Health <= 0) StartCoroutine(Die());
            else UpdateCardDisplay();
        }

        public void RescaleHealth(int newMaxHealth)
        {
            var difference = newMaxHealth - MaxHealth;
            MaxHealth = newMaxHealth;
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
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
            UpdateCardDisplay();
        }

        protected virtual IEnumerator Die()
        {
            if (!IsActive) yield break;
            IsActive = false;
            yield return StartCoroutine(AnimateCard(Vector3.one, Vector3.zero));
            Owner.Damage -= AmplifiedDamage;
            Owner.RecalculateDamage();
            Awake();
            Health = MaxHealth;
            RecalculateDamage();
            GetComponent<Image>().color = new Color(1f, 1f, 1f);
            WholeCardImage.localScale = Vector3.one;
            
            OnCardDead?.Invoke();
        }

        public virtual void UpdateCardDisplay()
        {
            if (NameText != null) NameText.text = Name;
            if (CostText != null) CostText.text = Cost.ToString();
            if (DamageText != null) DamageText.text = AmplifiedDamage.ToString();
            if (HealthText != null) HealthText.text = Health.ToString();
        }
        
        public virtual void HandleUpdate()
        {
            Owner.Damage -= AmplifiedDamage;
            RecalculateDamage();
            Owner.Damage += AmplifiedDamage;
            Owner.RecalculateDamage();
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
                IsRightButtonHeld = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                StartCoroutine(ShowWindowAfterDelay());
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                IsRightButtonHeld = false;
                Cursor.lockState = CursorLockMode.None;
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
                var newTransform = GameObject.FindGameObjectWithTag("AdditionalCanvas").transform;
                InfoScreen.transform.SetParent(newTransform);
                if (CardStats != null) CardStats.transform.localPosition = new Vector3(0, 0, 0);
                if (SpellDescription != null) SpellDescription.transform.localPosition = new Vector3(0, -181.8f, 0);
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
            if (WholeCardImage != null) WholeCardImage.localScale = startScale;

            while (elapsed < AnimationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / AnimationDuration);
                if (WholeCardImage != null) WholeCardImage.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }

            if (WholeCardImage != null) WholeCardImage.localScale = endScale;
        }
        
        protected virtual void CheckUpgrades(){}
        
        public virtual void ActiveSpell(Card target = null){}
        }
}
