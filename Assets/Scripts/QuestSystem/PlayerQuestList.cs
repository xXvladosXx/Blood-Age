using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using DialogueSystem.AIDialogue.AIDialogueConditions;
using Entity;
using InventorySystem;
using QuestSystem.QuestObjectives;
using QuestSystem.Quests;
using SaveSystem;
using StatsSystem;
using UnityEngine;

namespace QuestSystem
{
    public class PlayerQuestList : MonoBehaviour, ISavable, IPredicateEvaluator
    {
        [SerializeField] private List<QuestStatus> _statuses;
        public IEnumerable<QuestStatus> GetStatuses => _statuses;

        private AliveEntity _aliveEntity;
        private Objective _currentObjective;
        private ItemPicker _itemPicker;
        
        public event Action OnQuestUpdate;
        public event Action<Objective> OnObjectiveChange;
        private void Awake()
        {
            _itemPicker = GetComponent<ItemPicker>();
            _aliveEntity = GetComponent<AliveEntity>();
        }

        private void Start()
        {
            _aliveEntity.GetLevelData.OnEnemyDie += CheckKillingQuests;
        }

        private void Update()
        {
        }

        private void CheckKillingQuests(Class killedClass)
        {
            foreach (var status in _statuses)
            {
                foreach (var objective in status.GetQuest.GetObjectives)
                {
                    if (objective is KillingObjective killingObjective)
                    {
                        if (killedClass == killingObjective.GetClassToKill)
                        {
                            killingObjective.Amount++;
                            
                            if (killingObjective.CheckToComplete())
                            {
                                CompleteObjective(status.GetQuest, objective);
                            }
                        }
                    }
                }
            }
        }

        public void AddQuest(Quest quest)
        {
            QuestStatus questStatus = new QuestStatus(quest);
            
            if (_statuses.Any(status => status.GetQuest == quest)) return;
            
            _statuses.Add(questStatus);
            _currentObjective = quest.GetObjectives.FirstOrDefault();
            
            OnObjectiveChange?.Invoke(_currentObjective);
            OnQuestUpdate?.Invoke();
        }

        private QuestStatus GetQuestStatus(Quest quest) => _statuses.FirstOrDefault(status => status.GetQuest == quest);
        public bool HasQuest(Quest quest) => GetQuestStatus(quest) != null;
        
        public void CompleteObjective(Quest quest, Objective[] objectives)
        {
            QuestStatus questStatus = _statuses.First(status => status.GetQuest == quest);
            foreach (var objective in objectives)
            {
                questStatus.CompleteObjective(objective);
            }
            
            foreach (var status in _statuses)
            {
                foreach (var objective in status.GetQuest.GetObjectives)
                {
                    if (!status.IsObjectiveComplete(objective))
                    {
                        _currentObjective = objective;
                        OnObjectiveChange?.Invoke(_currentObjective);
                    }
                }
            }
            
            CompleteQuest(quest, questStatus);
            
            OnQuestUpdate?.Invoke();
        }

        private void CompleteQuest(Quest quest, QuestStatus questStatus)
        {
            if (questStatus.IsComplete)
            {
                GiveReward(quest);
            }
        }

        private void CompleteObjective(Quest quest, Objective objective)
        {
            QuestStatus questStatus = _statuses.First(status => status.GetQuest == quest);
            questStatus.CompleteObjective(objective);

            if (questStatus.IsComplete)
            {
                GiveReward(quest);
            }
            
            OnQuestUpdate?.Invoke();
        }

        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.GetRewards)
            {
                bool success = _itemPicker.GetItemContainer.AddItem(reward.Item.Data, reward.Amount);
                if (!success)
                {
                    Debug.Log("Too many items");
                }
            }
            
            OnObjectiveChange?.Invoke(null);
        }

        public object CaptureState() => 
            _statuses.Select(status => status.CaptureState())
                .ToList();

        public void RestoreState(object state)
        {
            if(!(state is List<object> stateList)) return;

            _statuses.Clear();
            foreach (var o in stateList)
            {
                _statuses.Add(new QuestStatus(o));
            }
        }

        public bool? Evaluate(AIDialogueQuestCondition parameters)
        {
            if (parameters == null) return true;
            if (!parameters.CheckCondition(this)) return false;

            return HasQuest(Quest.GetByName(parameters.GetQuest.name));
        }
    }
}