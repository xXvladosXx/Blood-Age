using EntitySpawnerSystem;
using UnityEngine;

namespace DialogueSystem.AIDialogue
{
    [CreateAssetMenu (menuName = "DialogueSystem/Channels/AIPortalChanel")]
    public class AIPortalChanel : AIDialogueChanel 
    {
        public override void Visit(AIConversant aiConversant, PlayerConversant playerConversant)
        {
            EntitySpawner.Instance.ActivatePortal(true);
        }
    }
}