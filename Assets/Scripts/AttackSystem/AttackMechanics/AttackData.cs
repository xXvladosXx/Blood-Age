using System.Collections.Generic;
using Entity;
using Extensions;
using Resistance;
using UnityEngine;

namespace AttackSystem.AttackMechanics
{
    public class AttackData
    {
        public AliveEntity Damager { get; set; }
        public AliveEntity PointTarget { get; set; }
        public List<AliveEntity> Entities { get; set; }
        public List<AliveEntity> Targets { get; set; }
        public SerializableDictionary ElementalDamage { get; set; }
        public float Damage { get; set; }
        public float MaxDamage { get; set; }
        public float CriticalChance { get; set; }
        public float CriticalDamage { get; set; }
        public float Accuracy { get; set; }
        public bool Vampiric { get; set; }
    }
}