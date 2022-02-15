using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class SettingsMenu : Menu
    {
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _backButton;
        
        public override void Initialize()
        {
            _backButton.onClick.AddListener(MainMenuSwitcher.ShowLast);
            _soundButton.onClick.AddListener(() => print("sound"));
        }
    }
}