using Entity.Classes;
using Extensions;
using UnityEngine;

namespace StatsSystem
{
    [CreateAssetMenu (menuName = "Stat/StatsValueContainer")]
    public class CharacteristicModifierContainer : ScriptableObject
    {
        [SerializeField] private StatModifyDictionary _statModify;
        public StatModifyDictionary GetStatModifiers => _statModify;
    }
}