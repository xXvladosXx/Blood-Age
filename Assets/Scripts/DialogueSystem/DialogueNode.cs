using System;
using System.Collections.Generic;
using DialogueSystem.AIDialogue;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "DialogueSystem/DialogueNode")]
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private string _text;
        [SerializeField] private List<string> _children = new List<string>();
        [SerializeField] private Rect _rect = new Rect(0, 0, 200, 100);
        [SerializeField] private bool _isPlayerSpeaking;
        [SerializeField] private AIDialogueChanel OnExitAction;
        [SerializeField] private AIDialogueChanel OnEnterAction;
        [SerializeField] private Condition _condition;
        [SerializeField] private bool _oneTimeVisited;

        public bool WasVisited { get; set; } = false;

        public Rect GetRect() => _rect;
        public string GetText() => _text;
        public List<string> GetTextChildren() => _children;
        public AIDialogueChanel GetOnEnterAction => OnEnterAction;
        public AIDialogueChanel GetOnExitAction => OnExitAction;
        public bool GetOneTimeVisit => _oneTimeVisited;
        public bool IsPlayerSpeaking() => _isPlayerSpeaking;

        public Condition GetCondition => _condition;
        private void OnValidate()
        {
            WasVisited = false;
        }

        public bool CheckCondition(IEnumerable<IPredicateEvaluator> predicateEvaluators)
        {
            if (_oneTimeVisited)
                return !WasVisited;
            
            return _condition.Check(predicateEvaluators);
        }
        

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            _rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }
        public void SetPlayerSpeaking(bool newIsPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            _isPlayerSpeaking = newIsPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }
        public void SetText(string newText)
        {
            if (newText != _text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                _text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            _children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            _children.Remove(childID);
            EditorUtility.SetDirty(this);
        }
#endif
        
    }
}