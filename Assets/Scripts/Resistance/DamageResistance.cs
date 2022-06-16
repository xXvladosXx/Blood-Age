using System;
using System.Collections;
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
    }
}