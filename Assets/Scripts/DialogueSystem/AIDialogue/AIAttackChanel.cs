using StateMachine.EnemyStates;
using UnityEngine;

namespace DialogueSystem.AIDialogue
{
    [CreateAssetMenu (menuName = "DialogueSystem/Channels/AIAttackChanel")]
    
    public class AIAttackChanel : AIDialogueChanel
    {
        public override void Visit(AIConversant aiConversant, PlayerConversant playerConversant)
        {
            aiConversant.gameObject.AddComponent<EnemyStateManager>();
            aiConversant.enabled = false;
        }
    }
}