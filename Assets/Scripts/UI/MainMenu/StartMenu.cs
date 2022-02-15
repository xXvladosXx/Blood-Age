using System;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class StartMenu : Menu
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _startNewGame;
        protected SavingHandler _savingHandler;

        public override void Initialize()
        {
            _savingHandler = FindObjectOfType<SavingHandler>();

            _backButton.onClick.AddListener(MainMenuSwitcher.ShowLast);
            _startNewGame.onClick.AddListener(StartNewGame);
        }
        
        public void CreateName(string saveFile)
        {
            _saveFile = saveFile;
        }
        
        private void StartNewGame()
        {
            _savingHandler.StartNewGame(_saveFile);
        }
    }
}