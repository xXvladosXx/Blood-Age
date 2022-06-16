using System;
using Entity;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class StartMenu : Menu
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _startNewGame;

        public override void Initialize()
        {

            _backButton.onClick.AddListener(MainMenuSwitcher.ShowLast);
            _startNewGame.onClick.AddListener(StartNewGame);
        }
        
        public void CreateName(string saveFile)
        {
            SaveFile = saveFile;
        }
        
        private void StartNewGame()
        {
            SavingHandler.StartNewGame(SaveFile);
        }

    }
}