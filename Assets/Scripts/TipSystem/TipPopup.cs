using System;
using PauseSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TipSystem
{
    public class TipPopup : MonoBehaviour, IPauseHandler
    {
        public static TipPopup Instance { get; private set; }
        
        [SerializeField] private TextMeshProUGUI _tipText;
        [SerializeField] private Button _exit;

        private void Awake()
        {
            Instance = this;
            
            _exit.onClick.AddListener(ResumeGame);
        }

        private void Start()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            } 
        }

        private void ResumeGame()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            } 
            
            SetPaused(false);
        }

        public void ActivateTip(string tipText)
        {
            _tipText.text = tipText;
            SetPaused(true);
            
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }    
        }
        
        public void SetPaused(bool isPaused)
        {
            ProjectContext.Instance.PauseManager.SetPaused(isPaused);
        }

        private void OnDisable()
        {
            _exit.onClick.RemoveAllListeners();
        }
    }
}