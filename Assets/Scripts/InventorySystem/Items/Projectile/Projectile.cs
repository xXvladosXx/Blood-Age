using UnityEngine;

namespace InventorySystem.Items.Projectile
{
    public abstract class Projectile : StatModifInventoryItem, IEquipable
    {
        [SerializeField] protected GameObject _prefab;
        public GameObject GetPrefab => _prefab;

        public InventoryItem GetItem()
        {
            return this;
        }
    }
}