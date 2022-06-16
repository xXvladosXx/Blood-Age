
using System.IO;
using System.Linq;
using Entity;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenu : Menu
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingButton;
        [SerializeField] private Button _quitButton;

        public override void Initialize()
        {
            if (SavingHandler.GetLastSave == null)
            {
                _continueButton.gameObject.SetActive(false);
            }
            
            _startButton.onClick.AddListener(() => MainMenuSwitcher.Show<StartMenu>());
            _loadButton.onClick.AddListener(() => MainMenuSwitcher.Show<LoadMenu>());
            _settingButton.onClick.AddListener(() => MainMenuSwitcher.Show<SettingsMenu>());
            _continueButton.onClick.AddListener(() => SavingHandler.Instance.ContinueGame(SavingHandler.GetLastSave));
            _quitButton.onClick.AddListener( () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
            });
        }
        }

    }
