using System;
using System.Collections.Generic;
using DialogueSystem;
using Entity;
using StateMachine;
using StateMachine.PlayerStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialogue
{
    public class DialoguePanel : Panel
    {
        [SerializeField] private TextMeshProUGUI _aiText;
        [SerializeField] private TextMeshProUGUI _conversantName;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _choiceButton;
        [SerializeField] private Transform _choiceRoot;
        [SerializeField] private GameObject _aiResponse;
        [SerializeField] private Button _quitButton;

        private PlayerConversant _playerConversant;

        public override void Initialize(AliveEntity aliveEntity)
        {
            _playerConversant = aliveEntity.GetComponent<PlayerConversant>();
            
            _playerConversant.OnConversationUpdate += UpdateUI;
            _playerConversant.OnLastNode += DisableDialoguePanel;
            UpdateUI();
        }

        private void DisableDialoguePanel()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            ChangeUI(this);
            
            _quitButton.onClick.AddListener(() => _playerConversant.Quit());
            _nextButton.onClick.AddListener(() => _playerConversant.Next());
        }
        
        private void UpdateUI()
        {
            gameObject.SetActive(_playerConversant.IsActive());
            if (!_playerConversant.IsActive()) return;

            _conversantName.text = _playerConversant.GetCurrentConversantName();
            _aiResponse.SetActive(!_playerConversant.IsChoosing());
            _choiceButton.gameObject.SetActive(_playerConversant.IsChoosing());

            if (_playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                foreach (Transform child in _choiceRoot)
                {
                    Destroy(child.gameObject);
                }

                _aiText.text = _playerConversant.GetText();
                _nextButton.gameObject.SetActive(_playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform child in _choiceRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (DialogueNode choiceNode in _playerConversant.GetChoices())
            {
                Button choice = Instantiate(_choiceButton, _choiceRoot);
                choice.GetComponentInChildren<TextMeshProUGUI>().text = choiceNode.GetText();
                choice.onClick.AddListener(() => { _playerConversant.SelectChoice(choiceNode); });
            }
        }

        private void OnDisable()
        {
            _playerConversant.TryGetComponent(out IStateSwitcher switcher);
            if(switcher.GetCurrentState is DialoguePlayerState dialoguePlayerState)
            {
                 dialoguePlayerState.EndDialogue();
            }
            _nextButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }
    }
}