﻿using DialogueSystem.AIDialogue.AIDialogueConditions;

namespace DialogueSystem
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(AIDialogueCondition parameters);
    }
}