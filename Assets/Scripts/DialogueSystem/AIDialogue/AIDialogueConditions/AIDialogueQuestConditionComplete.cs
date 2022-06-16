using System.Linq;
using QuestSystem;
using UnityEngine;

namespace DialogueSystem.AIDialogue.AIDialogueConditions
{
    [CreateAssetMenu (menuName = "DialogueSystem/DialogueConditionQuestComplete")]
    public class AIDialogueQuestConditionComplete : AIDialogueCondition
    {
        public override bool CheckCondition(PlayerQuestList playerQuestList)
        {
            return playerQuestList.GetStatuses.Where(status => status.GetQuest == _quest).Any(status => status.IsComplete);
        }
    }
}