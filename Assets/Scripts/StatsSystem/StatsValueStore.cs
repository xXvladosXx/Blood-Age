namespace StatsSystem
{
    using System;
    using System.Collections.Generic;
    using a;
    using DefaultNamespace;
    using DefaultNamespace.Entity.Class;
    using Entity.Class;
    using UnityEngine;

    public class StatsValueStore
    {
        private Dictionary<Characteristics, StatBonus> _statsAccordingToClass =
            new Dictionary<Characteristics, StatBonus>();

        private Dictionary<Stats, int> _assignedPoints = new Dictionary<Stats, int>();
        private Dictionary<Stats, int> _confirmedPoints = new Dictionary<Stats, int>();

        private int _unassignedPoints = 10;
        private int _defaultLevelUpPoints = 2;

        public int GetUnassignedPoints => _unassignedPoints;
        public event Action OnStatsChange;

        public StatsValueStore(FindStats findStats, BaseCharacterClass characterClass)
        {
            findStats.OnLevelUp += AddNewUnassignedPoints;
            var damageBonus = new StatBonus();
            var healthBonus = new StatBonus();
            var critDamageBonus = new StatBonus();
            var speedBonus = new StatBonus();

            _assignedPoints[Stats.Strength] = characterClass.Strength;
            _assignedPoints[Stats.Agility] = characterClass.Agility;
            _assignedPoints[Stats.Intelligence] = characterClass.Intelligence;

            switch (characterClass)
            {
                case Warrior warrior:
                    damageBonus.Bonus = 10;
                    damageBonus.Stat = Stats.Strength;
                    break;

                case Archer archer:
                    damageBonus.Bonus = 10;
                    damageBonus.Stat = Stats.Agility;

                    break;

                case Wizard wizard:
                    damageBonus.Bonus = 10;
                    damageBonus.Stat = Stats.Intelligence;

                    break;
                default:
                    break;
            }

            _statsAccordingToClass.Add(Characteristics.Damage, damageBonus);
            _statsAccordingToClass.Add(Characteristics.Health, healthBonus);
            _statsAccordingToClass.Add(Characteristics.CriticalDamage, critDamageBonus);
            _statsAccordingToClass.Add(Characteristics.MovementSpeed, speedBonus);
        }

        public float AddStat(Characteristics characteristic)
        {
            if (_statsAccordingToClass.TryGetValue(characteristic, out var characteristics))
            {
                return (GetPoints(characteristics.Stat) * characteristics.Bonus);
            }

            return 0;
        }

        private int GetPoints(Stats stat)
        {
            return _assignedPoints.ContainsKey(stat) ? _assignedPoints[stat] : 0;
        }

        private int GetConfirmedPoints(Stats stat)
        {
            return _confirmedPoints.ContainsKey(stat) ? _confirmedPoints[stat] : 0;
        }

        public int GetProposedPoints(Stats stat)
        {
            return GetPoints(stat) + GetConfirmedPoints(stat);
        }

        public void AssignPoints(Stats stat, int points)
        {
            if (!CanAssignPoints(stat, points)) return;

            _confirmedPoints[stat] = GetConfirmedPoints(stat) + points;
            _unassignedPoints -= points;
        }

        public bool CanAssignPoints(Stats stat, int points)
        {
            if (GetConfirmedPoints(stat) + points < 0) return false;
            if (_unassignedPoints < points) return false;

            return true;
        }

        public void Confirm()
        {
            foreach (var stat in _confirmedPoints.Keys)
            {
                _assignedPoints[stat] = GetProposedPoints(stat);
            }

            OnStatsChange?.Invoke();

            _confirmedPoints.Clear();
        }

        private void AddNewUnassignedPoints()
        {
            _unassignedPoints += _defaultLevelUpPoints;
        }
    }

    [Serializable]
    public class StatBonus
    {
        public Stats Stat;
        public float Bonus;
    }
}