using Entity;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class EscapeMenu : Menu
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _settingButton;
        [SerializeField] private Button _quitButton;
        
        public override void Initialize()
        {
            _loadButton.onClick.AddListener(() => MainMenuSwitcher.Show<LoadMenu>());
            _settingButton.onClick.AddListener(() => MainMenuSwitcher.Show<SettingsMenu>());
            _saveButton.onClick.AddListener(() => MainMenuSwitcher.Show<SaveMenu>());
            _quitButton.onClick.AddListener( () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
            });
        }

        private void OnEnable()
        {
            _resumeButton.interactable = !GameObject.FindGameObjectWithTag("Player").GetComponent<AliveEntity>().GetHealth.IsDead();
            _saveButton.interactable = !GameObject.FindGameObjectWithTag("Player").GetComponent<AliveEntity>().GetHealth.IsDead()
                && GameZoneManager.Instance.GetGameZone != GameZoneManager.GameZone.Battlezone;
        }
    }
}