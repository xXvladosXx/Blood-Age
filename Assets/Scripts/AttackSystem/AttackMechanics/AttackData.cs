namespace DefaultNamespace.MouseSystem
{
    using UnityEngine;

    public class AttackData
    {
        public Transform Damager { get; set; }
        public Transform Target { get; set; }
        public bool HeavyAttack { get; set; }
        public float Damage { get; set; }
        public float CriticalChance { get; set; }
        public float CriticalDamage { get; set; }
    }
}