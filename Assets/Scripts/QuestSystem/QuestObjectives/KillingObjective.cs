using System;
using StatsSystem;
using UnityEngine;

namespace QuestSystem.QuestObjectives
{
    [CreateAssetMenu (menuName = "Quests/Objectives/KillingObjective")]
    public class KillingObjective : Objective
    {
        [SerializeField] private Class _class;
        [SerializeField] private int _amountToKill;
        public Class GetClassToKill => _class;
        public int Amount { get; set; }

        public bool CheckToComplete()
        {
            if (Amount >= _amountToKill)
                return true;

            return false;
        }

        private void OnValidate()
        {
            Amount = 0;
        }

        
    }
    
}