﻿using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.AIDialogue.AIDialogueConditions;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class Condition
    {
        [SerializeField] private string _predicate;
        [SerializeField] private AIDialogueQuestCondition _parameters;

        public AIDialogueQuestCondition GetDialogueCondition => _parameters;
        
        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (var evaluator in evaluators)
            {
                var result = evaluator.Evaluate(_parameters);
                if (result != null)
                {
                    if (result == false) return false;
                }
            }

            return true;
        }
    }
}