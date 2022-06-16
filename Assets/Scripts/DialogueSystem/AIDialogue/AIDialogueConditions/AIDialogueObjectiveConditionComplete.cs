using System.Linq;
using QuestSystem;
using QuestSystem.QuestObjectives;
using UnityEngine;

namespace DialogueSystem.AIDialogue.AIDialogueConditions
{
    [CreateAssetMenu (menuName = "DialogueSystem/DialogueObjectiveCondition")]
    public class AIDialogueObjectiveConditionComplete : AIDialogueCondition
    {
        [SerializeField] private Objective _objective;
        
        public override bool CheckCondition(PlayerQuestList playerQuestList)
        {
            Debug.Log("playerQuestList.GetStatuses.Any(status => status.IsObjectiveCompleted(_objective))");
            return playerQuestList.GetStatuses.Any(status => status.IsObjectiveCompleted(_objective));
        }
    }
}