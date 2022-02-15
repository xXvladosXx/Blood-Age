using Entity;
using QuestSystem.Quests;
using StatsSystem;
using UnityEngine;

namespace QuestSystem.QuestObjectives
{
    [CreateAssetMenu (menuName = "Quests/Objectives/SpeakableObjective")]
    public class SpeakableObjective : Objective
    {
        [SerializeField] private Class _questGiver;

        public Class GetQuestGiver => _questGiver;
    }
}