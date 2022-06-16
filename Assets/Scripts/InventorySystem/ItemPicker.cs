using System;
using System.Collections.Generic;
using Entity;
using InventorySystem.Items;
using LootSystem;
using SaveSystem;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    using UnityEngine;

    public class ItemPicker : MonoBehaviour, ISavable
    {
        [SerializeField] private ItemContainer _itemContainer;
        public ItemContainer GetItemContainer => _itemContainer;

        public event Action<InventoryItem, int> OnItemPickUp;
        public void AddItem(ItemPickUp lootObject, int amount = 1)
        {
            if (lootObject == null) return;

            if (!_itemContainer.AddItem(new ItemData(lootObject.GetInventoryItem), amount))
            {
                return;
            }

            OnItemPickUp?.Invoke(lootObject.GetInventoryItem, amount);
            Destroy(lootObject.gameObject);
        }
        
        public object CaptureState()
        {
            var items = _itemContainer.GetAllSlots();
            return items;
        }

        public void RestoreState(object state)
        {
            var items = state as List<Slot>;
            _itemContainer.ClearInventory();
            foreach (var item in items)
            {
                var equipItem = _itemContainer.Database.GetItemByID(item.ItemData.Id);
                if (equipItem != null)
                {
                    _itemContainer.AddItem(equipItem.Data, item.Amount);
                }
            }
        }
    }
}