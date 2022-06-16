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
        [SerializeField] private Button _quit;
        [SerializeField] private Button _choiceButton;
        [SerializeField] private Transform _choiceRoot;
        [SerializeField] private GameObject _aiResponse;

        private PlayerConversant _playerConversant;

        public override void Initialize(AliveEntity aliveEntity)
        {
            _playerConversant = aliveEntity.GetComponent<PlayerConversant>();

            _playerConversant.OnDialogueStart += () => ChangeUI(this);
            _playerConversant.OnConversationUpdate += UpdateUI;
            
            UpdateUI();
        }

        private void OnEnable()
        {
            _nextButton.onClick.AddListener(() => _playerConversant.Next());
        }

        private void UpdateUI()
        {
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
                _quit.gameObject.SetActive(!_playerConversant.HasNext());
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
            _nextButton.onClick.RemoveAllListeners();
            _playerConversant.EndDialogue();
        }

        public void EndDialogue()
        {
            ChangeUI(this);
        }
    }
}