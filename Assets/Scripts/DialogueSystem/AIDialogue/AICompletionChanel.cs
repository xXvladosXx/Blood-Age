using QuestSystem;
using QuestSystem.QuestObjectives;
using QuestSystem.Quests;
using UnityEngine;

namespace DialogueSystem.AIDialogue
{
    [CreateAssetMenu (menuName = "DialogueSystem/Channels/AICompletionChanel")]
    public class AICompletionChanel : AIDialogueChanel
    {
        [SerializeField] private Quest _quest;
        [SerializeField] private Objective[] _objectives;
        
        public override void Visit(AIConversant aiConversant, PlayerConversant playerConversant)
        {
            playerConversant.GetComponent<PlayerQuestList>().CompleteObjective(_quest, _objectives);
        }
    }
}