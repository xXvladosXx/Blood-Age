using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using InventorySystem.Items;
using QuestSystem.QuestObjectives;
using UnityEngine;

namespace QuestSystem.Quests
{
    [CreateAssetMenu(menuName = "Quests/Quest")]
    public class Quest : ScriptableObject
    {
        [SerializeField] protected string _title;
        [SerializeField] protected List<Reward> _rewards = new List<Reward>();
        [SerializeField] protected float _experience;
        [SerializeField] protected List<Objective> _objectives = new List<Objective>();
        private static IEnumerable<Quest> _quests;

        [Serializable]
        public class Reward
        {
            [Min(1)]
            public int Amount;
            public InventoryItem Item;
        }
        
        public List<Objective> GetObjectives => _objectives;
        public string GetTitle => _title;
        public int GetProgress => _objectives.Count;
        public IEnumerable<Reward> GetRewards => _rewards;
        public float GetExperience => _experience;

        public Objective GetNextObjective(Objective objective)
        {
            int index = _objectives.FindIndex(a => a == objective);
            
            return index + 1 >= _objectives.Count ? null : _objectives[index + 1];
        }

        public bool HasObjective(Objective objective) => 
            _objectives.Any(obj => obj.Reference == objective.Reference);

        public static void GetAllQuests()
        {
            _quests = Resources.LoadAll<Quest>("");
        }
        
        public static Quest GetByName(string questName) => 
           _quests.FirstOrDefault(quest => quest.name == questName);
    }
}