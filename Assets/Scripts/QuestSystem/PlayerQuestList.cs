using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using DialogueSystem.AIDialogue.AIDialogueConditions;
using Entity;
using InventorySystem;
using InventorySystem.Items;
using QuestSystem.QuestObjectives;
using QuestSystem.Quests;
using SaveSystem;
using SceneSystem;
using StatsSystem;
using UnityEngine;

namespace QuestSystem
{
    [RequireComponent(typeof(PlayerEntity))]
    public class PlayerQuestList : MonoBehaviour, ISavable, IPredicateEvaluator
    {
        [SerializeField] private List<QuestStatus> _statuses;

        private Dictionary<KillingObjective, Quest> _killingObjectives = new Dictionary<KillingObjective, Quest>();

        private Dictionary<EquippingObjective, Quest>
            _equippingObjectives = new Dictionary<EquippingObjective, Quest>();

        private Dictionary<ReceivingObjective, Quest>
            _receivingObjectives = new Dictionary<ReceivingObjective, Quest>();

        private Dictionary<MoveToPortalObjective, Quest> _portalObjectives =
            new Dictionary<MoveToPortalObjective, Quest>();

        private List<Objective> _completedObjectives = new List<Objective>();
        private PlayerEntity _playerEntity;
        private Objective _currentObjective;

        public IEnumerable<QuestStatus> GetStatuses => _statuses;

        public event Action<Class> OnEntityRequest;
        public event Action<bool> OnPortalRequest;
        public event Action OnItemRequest;
        public event Action OnNullRequest;

        public event Action OnQuestUpdate;

        private void Awake()
        {
            _playerEntity = GetComponent<PlayerEntity>();
        }

        private void Start()
        {
            foreach (var status in _statuses)
            {
                FindCurrentObjectives(status.GetQuest);
            }

            SendObjectiveRequest();

            QuestStatus.LoadAllObjectives();
            Quest.GetAllQuests();
        }

        private void OnEnable()
        {
            _playerEntity.GetLevelData.OnEnemyDie += CheckKillingQuests;
            _playerEntity.GetItemEquipper.OnEquipmentChange += CheckEquippingQuests;
            _playerEntity.GetCustomer.OnItemBuy += CheckReceivingQuest;
            _playerEntity.GetItemPicker.OnItemPickUp += CheckReceivingQuest;
        }

        private void OnDisable()
        {
            _playerEntity.GetLevelData.OnEnemyDie -= CheckKillingQuests;
            _playerEntity.GetItemEquipper.OnEquipmentChange -= CheckEquippingQuests;
            _playerEntity.GetCustomer.OnItemBuy -= CheckReceivingQuest;
            _playerEntity.GetItemPicker.OnItemPickUp -= CheckReceivingQuest;
        }

        private QuestStatus GetQuestStatus(Quest quest) => _statuses.FirstOrDefault(status => status.GetQuest == quest);
        private bool HasQuest(Quest quest) => GetQuestStatus(quest) != null;

        private void CheckKillingQuests(Class killedClass)
        {
            foreach (var killingObjective in _killingObjectives.Keys)
            {
                if (killingObjective.GetClassToKill != killedClass) continue;

                killingObjective.Amount++;

                if (killingObjective.CheckToComplete())
                {
                    CompleteObjective(_killingObjectives[killingObjective], killingObjective);
                }
            }
        }

        private void CheckEquippingQuests(InventoryItem inventoryItem)
        {
            foreach (var equippingObjective in _equippingObjectives.Keys)
            {
                if (equippingObjective.GetItem == inventoryItem)
                {
                    CompleteObjective(_equippingObjectives[equippingObjective], equippingObjective);
                }
            }
        }

        private void CheckReceivingQuest(InventoryItem inventoryItem, int amount)
        {
            foreach (var receivingObjective in _receivingObjectives.Keys)
            {
                if (receivingObjective.GetItem == inventoryItem)
                {
                    if (amount >= 1)
                        CompleteObjective(_receivingObjectives[receivingObjective], receivingObjective);
                }
            }
        }


        public void CheckPortalQuests()
        {
            foreach (var portalObjective in _portalObjectives.Keys)
            {
                CompleteObjective(_portalObjectives[portalObjective], portalObjective);
                break;
            }

            if (_portalObjectives.Count < 1) return;
            _portalObjectives.Remove(_portalObjectives.Keys.First());
        }


        public void AddQuest(Quest quest)
        {
            QuestStatus questStatus = new QuestStatus(quest);

            if (_statuses.Any(status => status.GetQuest == quest)) return;

            _statuses.Add(questStatus);
            _currentObjective = quest.GetObjectives.FirstOrDefault();

            FindCurrentObjectives(quest);
            SendObjectiveRequest();

            OnQuestUpdate?.Invoke();
        }

        private void FindCurrentObjectives(Quest quest)
        {
            foreach (var objective in quest.GetObjectives)
            {
                if (_completedObjectives.Contains(objective)) continue;
                if (_currentObjective == null)
                    _currentObjective = objective;

                switch (objective)
                {
                    case KillingObjective killingObjective:
                        if (_killingObjectives.ContainsKey(killingObjective)) continue;
                        _killingObjectives.Add(killingObjective, quest);
                        break;
                    case EquippingObjective equippingObjective:
                        if (_equippingObjectives.ContainsKey(equippingObjective)) continue;
                        _equippingObjectives.Add(equippingObjective, quest);
                        break;
                    case MoveToPortalObjective moveToPortalObjective:
                        if (_portalObjectives.ContainsKey(moveToPortalObjective)) continue;
                        _portalObjectives.Add(moveToPortalObjective, quest);
                        break;
                    case ReceivingObjective receivingObjective:
                        if (_receivingObjectives.ContainsKey(receivingObjective)) continue;
                        _receivingObjectives.Add(receivingObjective, quest);
                        break;
                }
            }
        }


        private bool CompleteQuest(Quest quest, QuestStatus questStatus)
        {
            if (questStatus.IsComplete)
            {
                GiveReward(quest);
                return true;
            }

            return false;
        }

        public void CompleteObjective(Quest quest, Objective objective)
        {
            QuestStatus questStatus = _statuses.First(status => status.GetQuest == quest);
            if (questStatus.CompletedObjective(objective))
            {
                return;
            }

            if (CompleteQuest(quest, questStatus))
            {
                _completedObjectives.Add(objective);
                _currentObjective = null;
                OnQuestUpdate?.Invoke();
                SendObjectiveRequest();
                return;
            }

            _completedObjectives.Add(objective);
            FindCurrentObjectives(quest);

            _currentObjective = FindNextObjective(quest);
            SendObjectiveRequest();

            OnQuestUpdate?.Invoke();
        }

        private void SendObjectiveRequest()
        {
            switch (_currentObjective)
            {
                case MoveToPortalObjective moveToPortalObjective:
                    print("potawk");
                    OnPortalRequest?.Invoke(true);
                    break;
                case KillingObjective killingObjective:
                    OnEntityRequest?.Invoke(killingObjective.GetClassToKill);
                    break;
                case SpeakableObjective speakableObjective:
                    OnEntityRequest?.Invoke(speakableObjective.GetQuestGiver);
                    break;
                case EquippingObjective equippingObjective:
                    OnItemRequest?.Invoke();
                    break;
                default:
                    OnNullRequest?.Invoke();
                    break;
            }
        }

        private Objective FindNextObjective(Quest quest)
        {
            print(quest.GetNextObjective(_currentObjective));
            return quest.GetNextObjective(_currentObjective);
        }

        private void GiveReward(Quest quest)
        {
            _playerEntity.GetLevelData.ExperienceReward(quest.GetExperience);
            foreach (var reward in quest.GetRewards)
            {
                bool success = _playerEntity.GetItemPicker.GetItemContainer.AddItem(reward.Item.Data, reward.Amount);
                if (!success)
                {
                    Debug.Log("Too many items");
                }
            }
        }

        public object CaptureState() =>
            _statuses.Select(status => status.CaptureState())
                .ToList();

        public void RestoreState(object state)
        {
            if (!(state is List<object> stateList)) return;

            object lastQuest = null;
            _statuses.Clear();
            _completedObjectives.Clear();

            foreach (var o in stateList)
            {
                var questStatus = new QuestStatus(o);
                _statuses.Add(questStatus);

                _completedObjectives.AddRange(questStatus.GetCompletedObjectives);

                lastQuest = o;
            }

            FindCurrentObjectives(new QuestStatus(lastQuest).GetQuest);
            SendObjectiveRequest();
        }

        public bool? Evaluate(AIDialogueCondition parameters)
        {
            if (parameters == null) return true;
            if (!parameters.CheckCondition(this)) return false;

            return HasQuest(Quest.GetByName(parameters.GetQuest.name));
        }
    }

}