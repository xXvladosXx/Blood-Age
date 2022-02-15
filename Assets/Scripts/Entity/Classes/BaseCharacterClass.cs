using System;
using QuestSystem;
using StatsSystem;

namespace Entity.Classes
{
    [Serializable]
    public class BaseCharacterClass
    {
        public int Strength;
        public int Agility;
        public int Intelligence;
    }
    
    [Serializable]
    public class BaseStatBonus
    {
        public Stats Stat;
        public int Value;
    }
    
    [Serializable]
    public class ModifyStatBonus
    {
        public Stats Stat;
        public float Value;
    }
}