namespace InventorySystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Unity.Transforms;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Inventory/Inventory")]
    public class Inventory : ScriptableObject
    {
        public ItemDatabaseObject Database;
        public InventoryContainer InventoryContainer;

        public bool HasEnough()
        {
            return HasEmptySlot > 0;
        }

        public void AddItem(ItemData itemData, int amount)
        {
            var inventorySlot = FindItemInInventory(itemData);

            if (!Database.Items[itemData.Id].Stackable || inventorySlot == null)
            {
                for (int i = 0; i < amount; i++)
                {
                    SetEmptySlot(itemData, i);
                }
            }
            
        }

        public Item FindNecessaryItem(int id)
        {
            return Database.Items.FirstOrDefault(item => id == item.Data.Id);
        }

        public void SwapItem(InventorySlot draggedItem, InventorySlot replacedItem)
        {
            if (replacedItem.CanBeReplaced(FindNecessaryItem(draggedItem.ItemData.Id)) && 
                draggedItem.CanBeReplaced(FindNecessaryItem(replacedItem.ItemData.Id)))
            {
                var temp = new InventorySlot(replacedItem.ItemData, replacedItem.Amount);
                replacedItem.UpdateSlot(draggedItem.ItemData, draggedItem.Amount);
                draggedItem.UpdateSlot(temp.ItemData, temp.Amount);
            }
            
        }

        public void RemoveItem(int id, int amount = 0)
        {
            if (amount == 0)
            {
                foreach (var inventorySlot in InventoryContainer.InventorySlots)
                {
                    if (inventorySlot.ItemData.Id == id)
                    {
                        inventorySlot.UpdateSlot(new ItemData(), 0);
                        break;
                    }
                }
            }
            else
            {
                foreach (var inventorySlot in InventoryContainer.InventorySlots)
                {
                    if (inventorySlot.ItemData.Id == id)
                    {
                        inventorySlot.Amount -= amount;
                        break;
                    }
                }
            }
        }

        public List<Item> GetInventoryItems()
        {
            return (from inventorySlot in InventoryContainer.InventorySlots 
                from item in Database.Items 
                where inventorySlot.ItemData.Id == item.Data.Id 
                select item).ToList();
        }

        private InventorySlot SetEmptySlot(ItemData itemData, int amount)
        {
            foreach (var inventorySlot in InventoryContainer.InventorySlots)
            {
                if(inventorySlot.ItemData.Id > -1) continue;
                
                inventorySlot.UpdateSlot(itemData, amount);

                return inventorySlot;
            }

            return null;
        }

        private InventorySlot FindItemInInventory(ItemData itemData)
        {
            return InventoryContainer.InventorySlots.FirstOrDefault(slot => slot.ItemData.Id == itemData.Id);
        }

        private int HasEmptySlot
        {
            get
            {
                return InventoryContainer.InventorySlots.Count(inventorySlot => inventorySlot.ItemData.Id <= -1);
            }
        }
    }

    [Serializable]
    public class InventoryContainer
    {
        public InventorySlot[] InventorySlots = new InventorySlot[18];
    }

    [Serializable]
    public class InventorySlot
    {
        public ItemCategory[] ItemCategories = Array.Empty<ItemCategory>();
        public ItemData ItemData;
        public int Amount;

        public InventorySlot()
        {
            ItemData = null;
            Amount = 0;
        }

        public InventorySlot(ItemData itemData, int amount)
        {
            ItemData = itemData;
            Amount = amount;
        }

        public void UpdateSlot(ItemData itemData, int amount)
        {
            ItemData = itemData;
            Amount = amount;
        }

        public void AddAmount(int value)
        {
            Amount += value;
        }

        public bool CanBeReplaced(Item itemObject)
        {
            if (ItemCategories.Length <= 0 || itemObject == null || itemObject.Data.Id < 0)
            {
                return true;
            }

            var anyItem = ItemCategories.Any(t => itemObject.Category == t);

            return anyItem;
        }

        public void RemoveItem()
        {
            ItemData = new ItemData();
            Amount = 0;
        }
    }

    public enum ItemCategory
    {
        None,
        Pants,
        Helmet,
        Chest,
        Boots,
        Weapon,
        Ring,
        Potion
    }
}