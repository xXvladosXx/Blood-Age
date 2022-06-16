using System;
using Entity;
using InventorySystem.Items;
using MouseSystem;
using SaveSystem;
using StatsSystem;
using UnityEngine;

namespace LootSystem
{
    public class ItemPickUp : MonoBehaviour, IRaycastable
    {
        [SerializeField] private InventoryItem _inventoryItem;

        public InventoryItem GetInventoryItem => _inventoryItem;
        
        public int Amount { get; set; }
        
        public static event Action<ItemPickUp, Vector3, int> OnItemSpawn = delegate { };
        public static event Action<ItemPickUp> OnItemTake = delegate { };

        private void Start()
        {
            SpawnItem();
        }

        private void SpawnItem()
        {
            OnItemSpawn(this, transform.position + Vector3.up, Amount);
        }

        private void OnDestroy()
        {
            OnItemTake(this);
        }

        public CursorType GetCursorType() => CursorType.PickUp;
       

        public bool HandleRaycast(PlayerEntity player) => true;
       
    }
}