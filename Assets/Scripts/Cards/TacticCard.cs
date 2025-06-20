using System.Collections;
using BattleScene;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Cards
{
    public abstract class TacticCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public string Name { get; protected set; }
        public int Damage { get; protected set; }
        public int Side { get; protected set; }
        public bool IsActive { get; protected set; }
        public int Sector { get; set; }
        protected string CardDescription { get; set; }
        public Hero Owner { get; set; }
        
        protected TMP_Text NameText;
        protected TMP_Text DamageText;
        protected TMP_Text SpellDescription;
        protected TMP_Text CardStats;
        protected GameObject InfoScreen;
        protected bool IsRightButtonHeld;

        public UnityAction OnDisabled, OnInitialized;

        protected void Start()
        {
            InfoScreen = transform.Find("InfoScreen")?.gameObject;
            if (InfoScreen != null) InfoScreen.SetActive(true);
            NameText = transform.Find("TacticCardName")?.GetComponent<TMP_Text>();
            DamageText = transform.Find("TacticCardDamage")?.GetComponent<TMP_Text>();
            CardStats = transform.Find("InfoScreen/TacticCardStats")?.GetComponent<TMP_Text>();
            SpellDescription = transform.Find("InfoScreen/TacticCardDescription")?.GetComponent<TMP_Text>();
            if (CardStats != null) CardStats.text = $"{Name}\nУрон: {Damage}";
            if (SpellDescription != null) SpellDescription.text = CardDescription;
            if (NameText != null) NameText.text = Name;
            if (DamageText != null) DamageText.text = Damage.ToString();
            if (InfoScreen != null)InfoScreen.SetActive(false);
        }
        
        public void Initialize()
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            IsActive = true;
            Side = Owner.Side;
            Owner.Damage += Damage;
            Sector = Owner.Sector;
            Owner.RecalculateDamage();
            OnInitialized?.Invoke();
        }

        public void Disable()
        {
            IsActive = false;
            Owner.Damage -= Damage;
            Owner.RecalculateDamage();
            OnDisabled?.Invoke();
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
        
        private IEnumerator ShowWindowAfterDelay(float delay = 0.5f)
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
        
        public abstract void Attack(Card target);
        }
}
