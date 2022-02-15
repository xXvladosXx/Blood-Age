using QuestSystem;
using QuestSystem.Quests;
using UnityEngine;

namespace DialogueSystem.AIDialogue
{
    [CreateAssetMenu (menuName = "DialogueSystem/Channels/AIQuestChanel")]
    public class AIQuestChanel : AIDialogueChanel
    {
        [SerializeField] private Quest _quest;
        public override void Visit(AIConversant aiConversant, PlayerConversant playerConversant)
        {
            playerConversant.GetComponent<PlayerQuestList>().AddQuest(_quest);
        }
    }
}