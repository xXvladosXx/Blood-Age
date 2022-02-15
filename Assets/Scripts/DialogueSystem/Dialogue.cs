using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "DialogueSystem/Dialogue")]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<DialogueNode> _nodes = new List<DialogueNode>();
        [SerializeField] private Vector2 _offset = new Vector2(100, 0);

        private readonly Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

        private void OnValidate()
        {
            _nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                _nodeLookup[node.name] = node;
            }
        }

        public DialogueNode GetRootNode() => _nodes[0];

        public IEnumerable<DialogueNode> GetAllNodes() => _nodes;

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
            => from childID in parentNode.GetTextChildren()
                where _nodeLookup.ContainsKey(childID)
                select _nodeLookup[childID];

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialogue Node");
            AddNode(newNode);
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            _nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            if (parent != null)
            {
                parent.AddChild(newNode.name);
                newNode.SetPlayerSpeaking(!parent.IsPlayerSpeaking());
                newNode.SetPosition(parent.GetRect().position + _offset);
            }

            return newNode;
        }

        private void AddNode(DialogueNode newNode)
        {
            _nodes.Add(newNode);
            OnValidate();
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (_nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
        {
            return GetAllChildren(currentNode).Where(dialogueNode => dialogueNode.IsPlayerSpeaking());
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
        {
            return GetAllChildren(currentNode).Where(dialogueNode => !dialogueNode.IsPlayerSpeaking());
        }
    }
}