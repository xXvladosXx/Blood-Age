using System.Linq;
using Entity.Classes;
using Extensions;
using QuestSystem;
using SaveSystem;

namespace StatsSystem
{
    using System;
    using System.Collections.Generic;
    using DefaultNamespace;
    using UnityEngine;

    public class StatsValueStore : ISavable
    {
        private StatModifyDictionary _statsAccordingToClass;

        private Dictionary<Stats, int> _assignedPoints = new Dictionary<Stats, int>();
        private Dictionary<Stats, int> _transitionPoints = new Dictionary<Stats, int>();
        
        private CharacteristicModifierContainer _characteristicModifierContainer;
        private StatStartContainer _statStartContainer;
        private FindStats _findStats;
        
        private int _unassignedPoints = 18;
        private int _defaultLevelUpPoints = 5;
        private int _lastLevel;
        
        public int GetUnassignedPoints => _unassignedPoints;
        public event Action OnStatsChange;
        

        public StatsValueStore(FindStats findStats, CharacteristicModifierContainer characteristicModifierContainer, StatStartContainer statStartContainer)
        {
            findStats.OnLevelUp += AddNewUnassignedPoints;

            _findStats = findStats;
            _characteristicModifierContainer = characteristicModifierContainer;
            _statStartContainer = statStartContainer;
            if(characteristicModifierContainer != null) 
                _statsAccordingToClass = characteristicModifierContainer.GetStatModifiers;

            if (_characteristicModifierContainer == null) return;
            
            foreach (var statBonus in statStartContainer.GetStatBonuses)
            {
                switch(statBonus.Stat)
                {
                    case Stats.Strength:
                        _assignedPoints[Stats.Strength] = statBonus.Value;
                        break;
                    case Stats.Intelligence:
                        _assignedPoints[Stats.Agility] = statBonus.Value;
                        break;
                    case Stats.Agility:
                        _assignedPoints[Stats.Intelligence] = statBonus.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public float GetCalculatedStat(Characteristics characteristic)
        {
            if (_characteristicModifierContainer == null) return 0;
            
            if(_statsAccordingToClass.TryGetValue(characteristic, out var c))
            {
                if (_assignedPoints == null) return 0;
                foreach (var assignedPoint in _assignedPoints)
                {
                    if (assignedPoint.Key == c.Stat)
                    {
                        float f = assignedPoint.Value * c.Value;
                        return f;
                    }
                }

                return 0;
            }
            return 0;
        }

        private int GetPoints(Stats stat)
        {
            return _assignedPoints.ContainsKey(stat) ? _assignedPoints[stat] : 0;
        }

        private int GetPointInTransition(Stats stat)
        {
            return _transitionPoints.ContainsKey(stat) ? _transitionPoints[stat] : 0;
        }

        public int GetProposedPoints(Stats stat)
        {
            return GetPoints(stat) + GetPointInTransition(stat);
        }

        public void AssignPoints(Stats stat, int points)
        {
            if (!CanAssignPoints(stat, points)) return;

            _transitionPoints[stat] = GetPointInTransition(stat) + points;
            _unassignedPoints -= points;
        }

        public bool CanAssignPoints(Stats stat, int points)
        {
            if (GetPointInTransition(stat) + points < 0) return false;
            if (_unassignedPoints < points) return false;

            return true;
        }

        public void Confirm()
        {
            foreach (var stat in _transitionPoints.Keys)
            {
                _assignedPoints[stat] = GetProposedPoints(stat);
            }

            OnStatsChange?.Invoke();

            _transitionPoints.Clear();
        }

        private void AddNewUnassignedPoints()
        {
            if (_statStartContainer == null) return;
            if(_lastLevel >= _findStats.GetLevel) return;
            
            foreach (var statBonus in _statStartContainer.GetStatBonuses)
            {
                switch (statBonus.Stat)
                {
                    case Stats.Strength:
                        _assignedPoints[Stats.Strength] += statBonus.Value;
                        break;
                    case Stats.Intelligence:
                        _assignedPoints[Stats.Intelligence] += statBonus.Value;
                        break;
                    case Stats.Agility:
                        _assignedPoints[Stats.Agility] += statBonus.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            _unassignedPoints += _defaultLevelUpPoints;
        }

        public object CaptureState()
        {
            var storedValues = new StoredStats
            {
                AssignedPoints = _assignedPoints,
                UnassignedPoints = _unassignedPoints,
                LastLevel = _findStats.GetLevel
            };

            foreach (var stat in _transitionPoints.Keys)
            {
                storedValues.UnassignedPoints += _transitionPoints[stat];
            }
            
            return storedValues;
        }

        public void RestoreState(object state)
        {
            var storedValues = (StoredStats) state;

            _assignedPoints = storedValues.AssignedPoints;
            _lastLevel = storedValues.LastLevel;
            _unassignedPoints = storedValues.UnassignedPoints;
        }
        
        [Serializable]
        public class StoredStats
        {
            public Dictionary<Stats, int> AssignedPoints;
            public int UnassignedPoints;
            public int LastLevel;
        }
    }
}