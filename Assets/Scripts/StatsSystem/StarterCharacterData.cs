using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatsSystem
{
    public enum Class
    {
        Archer,
        Warrior,
        Wizard, 
        OrdinaryWarrior,
        OrdinaryArcher,
        Guard,
        Ivan,
        None
    }

    public enum Characteristics 
    {
        Health,
        Damage, 
        MovementSpeed,
        AttackSpeed,
        HealthRegeneration,
        Mana,
        ManaRegeneration,
        CriticalChance,
        CriticalDamage,
        ExperienceToLevelUp,
        DeathExperience, 
        Evasion,
        Accuracy,
        Stamina,
        StaminaRegeneration,
    }

    public enum Stats
    {
        Strength,
        Intelligence,
        Agility
    }

    [Serializable]
    public class ClassCharacteristicsData
    {
        public Characteristics Characteristics;
        public float[] Value;
    }
    
    
    [Serializable]
    public class Character
    {
        public Class Class;
        public ClassCharacteristicsData[] Data;
    }
    
    [CreateAssetMenu (fileName = "CharacterData")]
    public class StarterCharacterData : ScriptableObject
    {
        [SerializeField] private Character[] _class;

        private Dictionary<Class, Dictionary<Characteristics, float[]>> _classData;

        public Dictionary<Class, Dictionary<Characteristics, float[]>> GetClassData => _classData;

        private void CreateData()
        {
            if(_classData != null) return;

            _classData = new Dictionary<Class, Dictionary<Characteristics, float[]>>();

            foreach (var character in _class)
            {
                var stats = new Dictionary<Characteristics, float[]>();

                foreach (var classCharacteristicsData in character.Data)
                {
                    stats[classCharacteristicsData.Characteristics] = classCharacteristicsData.Value;
                }

                _classData[character.Class] = stats;
            }
        }

        public float ReturnLevelValueCharacteristics(Class classChooser, Characteristics characteristics, int level)
        {
            CreateData();

            float[] levels = _classData[classChooser][characteristics];

            if (levels.Length <= 0)
            {
                return 0;
            }

            return levels[level - 1];
        }
        
        public int GetLevels(Class classChooser, Characteristics characteristics)
        {
            CreateData();
        
            float[] levels = _classData[classChooser][characteristics];
            return levels.Length;
        }
    }
}