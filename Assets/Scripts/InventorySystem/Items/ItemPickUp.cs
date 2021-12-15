namespace InventorySystem
{
    using System;
    using AttackSystem.Weapon;
    using UnityEngine;

    public class ItemPickUp : MonoBehaviour, IEquipable
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _amount;
        public int GetAmount => _amount;
        
        public Item GetItem()
        {
            Destroy(gameObject);
            return _item;
        }
    }

    
}