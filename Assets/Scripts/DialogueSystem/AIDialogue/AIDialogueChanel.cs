using UnityEngine;

namespace DialogueSystem.AIDialogue
{
    public abstract class AIDialogueChanel : ScriptableObject
    {
        public abstract void Visit(AIConversant aiConversant, PlayerConversant playerConversant);
    }
}