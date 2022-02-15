using Entity;
using QuestSystem;
using UnityEngine;

namespace DialogueSystem.AIDialogue.AIDialogueConditions
{
    public abstract class AIDialogueCondition : ScriptableObject
    {
        public abstract bool CheckCondition( PlayerQuestList playerQuestList);
    }
}