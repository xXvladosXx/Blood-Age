using Entity.Classes;
using UnityEngine;

namespace StatsSystem
{
    [CreateAssetMenu (menuName = "Stat/StatBonusContainer")]
    public class StatStartContainer : ScriptableObject
    {
        [SerializeField] private Class _class;
        [SerializeField] private BaseStatBonus[] _baseStatBonuses;

        public Class GetClass => _class;
        public BaseStatBonus[] GetStatBonuses => _baseStatBonuses;
    } 
}