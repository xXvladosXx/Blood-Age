using System.Linq;
using QuestSystem;
using QuestSystem.Quests;
using UnityEngine;

namespace DialogueSystem.AIDialogue.AIDialogueConditions
{
    [CreateAssetMenu (menuName = "DialogueSystem/DialogueCondition")]
    public class AIDialogueQuestCondition : AIDialogueCondition
    {
        [SerializeField] private Quest _quest;

        public Quest GetQuest => _quest;
        
        public override bool CheckCondition(PlayerQuestList playerQuestList) => playerQuestList.GetStatuses.Any(status => status.GetQuest == _quest);
    }
}