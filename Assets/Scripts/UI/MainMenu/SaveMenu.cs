using Entity;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class SaveMenu: Menu
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _saveNewGameButton;
        [SerializeField] private Transform _content;

        public override void Initialize()
        {
            _backButton.onClick.AddListener(MainMenuSwitcher.ShowLast);
            _saveNewGameButton.onClick.AddListener(() => MainMenuSwitcher.Show<CreateSaveMenu>());
        }

        private void OnEnable()
        {
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var save in SavingHandler.Instance.SaveList())
            {
                Button savePrefab = Instantiate(_saveButton, _content);
                savePrefab.GetComponentInChildren<TextMeshProUGUI>().text = save;
            
                savePrefab.onClick.AddListener((() => { SavingHandler.Instance.OverwriteGame(save); }));
            }
        }

    }
}