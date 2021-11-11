namespace InventorySystem
{
    using System;
    using AttackSystem.Weapon;
    using UnityEngine;

    public class ItemPickUp : MonoBehaviour, IEquipable
    {
        [SerializeField] private Item _item;
        [SerializeField] private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public Item Equip()
        {
            Destroy(gameObject);
            return _item;
        }
    }

    
}