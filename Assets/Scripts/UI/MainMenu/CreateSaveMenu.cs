using Entity;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class CreateSaveMenu : Menu
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _startNewGame;
        [SerializeField] private TextMeshProUGUI _warning;

        public override void Initialize()
        {
            _backButton.onClick.AddListener(MainMenuSwitcher.ShowLast);
            _startNewGame.onClick.AddListener(SaveNewGame);
        }

        public void CreateName(string saveFile)
        {
            SaveFile = saveFile;
        }

        private void SaveNewGame()
        {
            if (SaveFile.Length > 0)
            {
                _warning.text = "";
                SavingHandler.Save(SaveFile);
                MainMenuSwitcher.ShowLast();
            }
            else
            {
                _warning.text = "Fill the name";
            }
        }
    }
}