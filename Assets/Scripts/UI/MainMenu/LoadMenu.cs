using System;
using Entity;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class LoadMenu : Menu
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Transform _content;
        public override void Initialize()
        {
            _backButton.onClick.AddListener(MainMenuSwitcher.ShowLast);
        }

        private void OnEnable()
        {
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var save in SavingHandler.Instance.SaveList())
            {
                Button loadPrefab = Instantiate(_loadButton, _content);
                loadPrefab.GetComponentInChildren<TextMeshProUGUI>().text = save;
            
                loadPrefab.onClick.AddListener((() => { SavingHandler.Instance.LoadGame(save); }));
            }
        }

        
    }
}