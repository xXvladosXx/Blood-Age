using System;
using Entity.Classes;
using QuestSystem;
using Resistance;
using RotaryHeart.Lib.SerializableDictionary;
using StatsSystem;

namespace Extensions
{
    [Serializable]
    public class SerializableDictionary : SerializableDictionaryBase<DamageType, float> { }
    
    [Serializable] 
    public class StatModifyDictionary : SerializableDictionaryBase<Characteristics, ModifyStatBonus> {}
}