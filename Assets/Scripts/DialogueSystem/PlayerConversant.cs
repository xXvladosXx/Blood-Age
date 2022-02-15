using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.AIDialogue;
using StateMachine.PlayerStates;
using UnityEngine;

namespace DialogueSystem
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private Dialogue _testDialogue;

        private Dialogue _currentDialogue;
        private DialogueNode _currentNode;
        private bool _isChoosing;
        private AIConversant _currentAiConversant;

        public event Action OnConversationUpdate;
        public event Action OnLastNode;
        public string GetText() => _currentNode == null ? "" : _currentNode.GetText();
        public bool IsChoosing() => _isChoosing;

        public bool HasNext() => FilterOnCondition(_currentDialogue.GetAllChildren(_currentNode)).Any();

        public IEnumerable<DialogueNode> GetChoices() => FilterOnCondition(_currentDialogue.GetPlayerChildren(_currentNode));
        public bool IsActive() => _currentDialogue != null;
        public string GetCurrentConversantName() => _currentAiConversant.GetName;

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            _currentAiConversant = newConversant;
            _currentDialogue = newDialogue;
            _currentNode = _currentDialogue.GetRootNode();
            TriggerEnterAction();
            OnConversationUpdate?.Invoke();
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            _currentNode = chosenNode;

            if (_currentNode.GetOneTimeVisit)
                _currentNode.WasVisited = true;    
                
            TriggerEnterAction();
            _isChoosing = false;
            Next();
        }

        public void Next()
        {
            var numPlayerResponses = FilterOnCondition(_currentDialogue.GetPlayerChildren(_currentNode)).ToArray().Length;
            if (numPlayerResponses > 0)
            {
                _isChoosing = true;
                TriggerExitAction();
                OnConversationUpdate?.Invoke();
                return;
            }

            var dialogueNodes = FilterOnCondition(_currentDialogue.GetAIChildren(_currentNode)).ToArray();

            TriggerExitAction();

            if (dialogueNodes.Length > 0)
            {
                _currentNode = dialogueNodes[0];
            }
            else
            {
                GetComponent<PlayerStateManager>().SwitchState<DialoguePlayerState>(0).EndDialogue();
                OnLastNode?.Invoke();
                TriggerEnterAction();
            }
            
            TriggerEnterAction();
            
            OnConversationUpdate?.Invoke();
        }

        public void Quit()
        {
            _currentDialogue = null;
            TriggerExitAction();
            _currentNode = null;
            _currentAiConversant = null;
            _isChoosing = false;
            OnConversationUpdate?.Invoke();
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if(node.CheckCondition(GetEvaluators()))
                    yield return node;
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        private void TriggerEnterAction()
        {
            if (_currentNode != null)
            {
                TriggerAction(_currentNode.GetOnEnterAction);
            }
        }

        private void TriggerExitAction()
        {
            if (_currentNode != null)
            {
                TriggerAction(_currentNode.GetOnExitAction);
            }
        }

        private void TriggerAction(AIDialogueChanel action)
        {
            if(action == null) return;

            action.Visit(_currentAiConversant, this);
        }

    }
}