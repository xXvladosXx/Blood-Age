using System;
using System.Collections.Generic;
using System.Linq;
using QuestSystem.QuestObjectives;
using QuestSystem.Quests;
using UnityEngine;

namespace QuestSystem
{
    [Serializable]
    public class QuestStatus
    {
        [SerializeField] private Quest _quest;
        [SerializeField] private List<Objective> _completedObjectives = new List<Objective>();
        private static IEnumerable<Objective> _allObjectives;

        public Quest GetQuest => _quest;
        public int GetCompletedObjectivesCount => _completedObjectives?.Count ?? 0;
        public List<Objective> GetCompletedObjectives => _completedObjectives;

        public bool IsComplete => _quest.GetObjectives
            .All(objective => _completedObjectives.Contains(objective));

        public bool IsObjectiveCompleted(Objective objective) => _completedObjectives.Contains(objective);
        
        public static void LoadAllObjectives() => _allObjectives = Resources.LoadAll<Objective>("Objectives");

        [Serializable]
        public class QuestStatusRecord
        {
            public string QuestName { get; set; }
            public List<string> CompletedObjectives { get; set; }
        }

        public QuestStatus(Quest quest)
        {
            _quest = quest;
        }

        public QuestStatus(object quest)
        {
            QuestStatusRecord questStatusRecord = quest as QuestStatusRecord;
            _quest = Quest.GetByName(questStatusRecord.QuestName);

            foreach (var objective in questStatusRecord.CompletedObjectives)
            {
                foreach (var sObjective in _allObjectives)
                {
                    if (sObjective.name == objective)
                        _completedObjectives.Add(sObjective);
                }
            }
        }

        public bool CompletedObjective(Objective objective)
        {
            if (_completedObjectives.Contains(objective)) return true;
            if (_quest.HasObjective(objective))
            {
                _completedObjectives.Add(objective);
                return false;
            }

            return true;
        }

        public object CaptureState()
        {
            var statusRecord = new QuestStatusRecord
            {
                QuestName = _quest.name,
                CompletedObjectives = new List<string>()
            };

            foreach (var objective in _completedObjectives)
            {
                statusRecord.CompletedObjectives.Add(objective.name);
            }

            return statusRecord;
        }
    }
}