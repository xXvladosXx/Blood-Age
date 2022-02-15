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

        public event Action<Objective> OnObjectiveComplete;
        public Quest GetQuest => _quest;
        public int GetCompletedObjectives => _completedObjectives?.Count ?? 0;
        
        public bool IsComplete => _quest.GetObjectives
            .All(objective => _completedObjectives.Contains(objective));

        public bool IsObjectiveComplete(Objective objective) => _completedObjectives.Contains(objective);
        
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
            var sObjectives=  Resources.LoadAll<Objective>("Objectives");

            foreach (var objective in questStatusRecord.CompletedObjectives)
            {
                foreach (var sObjective in sObjectives)
                {
                    if(sObjective.name == objective)
                        _completedObjectives.Add(sObjective);
                }               
            }
        }
        
        public void CompleteObjective(Objective objective)
        {
            if (_quest.HasObjective(objective))
            {
                _completedObjectives.Add(objective);
                OnObjectiveComplete?.Invoke(objective);
            }
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