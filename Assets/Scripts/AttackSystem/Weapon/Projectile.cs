namespace AttackSystem.Weapon
{
    using DefaultNamespace;using InventorySystem;
    using UnityEngine;

    public abstract class Projectile : Item
    {
        [SerializeField] protected float _damage;
        [SerializeField] protected GameObject _prefab;
        public GameObject GetPrefab => _prefab;
        public float AddBonus(Characteristics characteristics)
        {
            if (characteristics == Characteristics.Damage)
                return _damage;

            return 0;
        }
    }
}