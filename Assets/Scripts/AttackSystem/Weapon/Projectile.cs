namespace AttackSystem.Weapon
{
    using DefaultNamespace;using InventorySystem;
    using UnityEngine;

    public abstract class Projectile : StatModifItem, IEquipable
    {
        [SerializeField] protected GameObject _prefab;
        public GameObject GetPrefab => _prefab;

        public Item GetItem()
        {
            return this;
        }
    }
}