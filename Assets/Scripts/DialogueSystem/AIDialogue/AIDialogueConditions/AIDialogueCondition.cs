using Entity;
using QuestSystem;
using QuestSystem.Quests;
using UnityEngine;

namespace DialogueSystem.AIDialogue.AIDialogueConditions
{
    public abstract class AIDialogueCondition : ScriptableObject
    {
        [SerializeField] protected Quest _quest;

        public Quest GetQuest => _quest;
        public abstract bool CheckCondition(PlayerQuestList playerQuestList);
    }
}