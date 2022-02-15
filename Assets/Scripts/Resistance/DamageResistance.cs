using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Resistance
{
    public enum DamageType
    {
        Fire,
        Ice,
        Physical, 
        Ground,
        Thunder
    }
    [CreateAssetMenu(fileName = "Damage Resistance", menuName = "Resistance")]
    public class DamageResistance : ScriptableObject
    {
        [Serializable]
        public class Resistance
        {
            public DamageType DamageType;
            [Range(-1, 1)]
            public float DamageResistance;
        }

        public List<Resistance> _settings = new List<Resistance>();
        public Dictionary<DamageType, float> _resistances = new Dictionary<DamageType, float>();

        public void SetUp()
        {
            foreach (var resistance in _settings)
            {
                if (!_resistances.TryGetValue(resistance.DamageType, out var f))
                {
                    _resistances.Add(resistance.DamageType, resistance.DamageResistance);
                }
                else
                {
                    _resistances.Remove(resistance.DamageType);
                    _resistances.Add(resistance.DamageType, f+resistance.DamageResistance);
                }
            }
        }

        public float CalculateResistance(float damage, DamageType damageType)
        {
            foreach (var resistance in _resistances)
            {
                if (resistance.Key == damageType)
                {
                    float f = damage * (1 - resistance.Value);
                    return f;
                }
            }

            return 0;
        }
    }
}