namespace AttackSystem.Weapon
{
    using DefaultNamespace;using InventorySystem;
    using UnityEngine;

    public abstract class Projectile : StatModifItem
    {
        [SerializeField] protected GameObject _prefab;
        public GameObject GetPrefab => _prefab;
      
    }
}