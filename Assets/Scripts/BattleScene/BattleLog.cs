using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScene
{
    public class BattleLog : MonoBehaviour
    {
        [SerializeField] private Transform logContent;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject logEntryPrefab;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private int maxEntries = 100;

    
        private List<GameObject> _logEntries = new List<GameObject>();

        public void AddLogMessage(string message)
        {
            var entryObj = Instantiate(logEntryPrefab, logContent);
            var textComponent = entryObj.GetComponentInChildren<TextMeshProUGUI>();
            
            var turnNumber = Math.Ceiling(gameManager.GetTurnNumber() / 2f);
            textComponent.text = $"[{turnNumber}] {message}";
        
            UpdateEntryHeight(entryObj, textComponent);
            
            _logEntries.Add(entryObj);
            
            if (_logEntries.Count > maxEntries)
            {
                RemoveOldestEntry();
            }
        
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
        
        private void UpdateEntryHeight(GameObject entry, TextMeshProUGUI text)
        {
            Canvas.ForceUpdateCanvases();
        
            var preferredHeight = text.preferredHeight;
            var layoutElement = entry.GetComponent<LayoutElement>();
            
            layoutElement.minHeight = preferredHeight + 10;
            layoutElement.preferredHeight = preferredHeight + 10;
        }
    
        private void RemoveOldestEntry()
        {
            if (_logEntries.Count == 0) return;
        
            GameObject oldest = _logEntries[0];
            _logEntries.RemoveAt(0);
            Destroy(oldest);
        }
        
        public void ClearLog()
        {
            foreach (var entry in _logEntries)
            {
                Destroy(entry);
            }
            _logEntries.Clear();
        }
    }
}