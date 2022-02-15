namespace InventorySystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [CreateAssetMenu(menuName = "Inventory/Inventory")]
    public abstract class ItemContainer : ScriptableObject
    {
        public ItemDatabaseObject Database;
        public Container Container;
        protected List<Func<ItemData, int, bool>> Filters = new List<Func<ItemData, int, bool>>();

        public event Action<Item, Item> OnItemChange;
        public event Action OnItemAdd;

        protected abstract void RegisterFilters();

        private void OnValidate()
        {
            RegisterFilters();
        }

        private bool HasEnough() => HasEmptySlot > 0;

        public bool AddItem(ItemData itemData, int amount)
        {
            if (!HasEnough()) return false;
            if (!Filters.Select(f => f(itemData, amount)).All(e => e)) return false;
            
            var inventorySlot = FindSlotInInventory(itemData);

            if ((Database.Items[itemData.Id].Stackable && inventorySlot != null))
            {
                inventorySlot.AddAmount(amount);
                return true;
            }
            
            SetSlot(itemData, amount);
            OnItemAdd?.Invoke();
            return true;
        }

        public Item FindNecessaryItemInData(int id)
        {
            return Database.Items.FirstOrDefault(item => id == item.Data.Id);
        }

        public bool SwapItem(Slot draggedItem, Slot replacedItem)
        {
            if (!draggedItem.CanBeChanged && replacedItem.CanBeChanged)
            {
                if (replacedItem.CanBeReplaced(FindNecessaryItemInData(draggedItem.ItemData.Id)))
                {
                    replacedItem.UpdateSlot(draggedItem.ItemData, draggedItem.Amount);
                    OnItemChange?.Invoke(FindNecessaryItemInData(draggedItem.ItemData.Id),FindNecessaryItemInData(replacedItem.ItemData.Id) );
                }
                
                return false;
            }
            
            if (!draggedItem.CanBeReplaced(FindNecessaryItemInData(replacedItem.ItemData.Id))) return false;
            if(!replacedItem.CanBeReplaced(FindNecessaryItemInData(draggedItem.ItemData.Id))) return false;

            var temp = new Slot(replacedItem.ItemData, replacedItem.Amount);
            if(!draggedItem.Discard)
                replacedItem.UpdateSlot(draggedItem.ItemData, draggedItem.Amount);
            
            if(!replacedItem.Discard)
                draggedItem.UpdateSlot(temp.ItemData, temp.Amount);
                
            OnItemChange?.Invoke(FindNecessaryItemInData(draggedItem.ItemData.Id),FindNecessaryItemInData(replacedItem.ItemData.Id) );

            return true;
        }

        public void ClearInventory()
        {
            foreach (var inventorySlot in Container.InventorySlots)
            {
                inventorySlot.UpdateSlot(new ItemData(), 0);
            }
        }

        public void RemoveItem(int id, int amount = 0)
        {
            if (amount == 0)
            {
                foreach (var inventorySlot in Container.InventorySlots)
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
                foreach (var inventorySlot in Container.InventorySlots)
                {
                    if (inventorySlot.ItemData.Id == id)
                    {
                        inventorySlot.Amount -= amount;
                        if(inventorySlot.Amount <= 0 && FindNecessaryItemInData(id).Stackable)
                            inventorySlot.UpdateSlot(new ItemData(), 0);
                        
                        break;
                    }
                }
            }
        }

        public List<Slot> GetAllSlots()
        {
            return Container.InventorySlots.ToList();
        }
        public List<int> GetAllItems()
        {
            List<int> items = new List<int>();

            foreach (var inventorySlot in Container.InventorySlots)
            {
                items.Add(inventorySlot.ItemData.Id);
            }

            return items;
        }

        public List<Item> GetInventoryItems()
        {
            List<Item> list = new List<Item>();
            foreach (Slot inventorySlot in Container.InventorySlots)
            foreach (var item in Database.Items)
            {
                if (inventorySlot.ItemData.Id == item.Data.Id) list.Add(item);
            }

            return list;
        }

        private Slot SetSlot(ItemData itemData, int amount)
        {
            foreach (var inventorySlot in Container.InventorySlots)
            {
                if (inventorySlot.ItemData.Id > -1) continue;

                inventorySlot.UpdateSlot(itemData, amount);
                
                return inventorySlot;
            }

            return null;
        }

        public Slot FindSlotInInventory(ItemData itemData)
        {
            return Container.InventorySlots.FirstOrDefault(slot => slot.ItemData.Id == itemData.Id);
        }
        public bool HasItemInInventory(ItemData itemData)
        {
            return Container.InventorySlots.Any(inventorySlot => inventorySlot.ItemData.Id == itemData.Id);
        }
        private int HasEmptySlot
        {
            get { return Container.InventorySlots.Count(inventorySlot => inventorySlot.ItemData.Id <= -1); }
        }

        public Slot GetSlotOfItem(Item item) => Container.InventorySlots
            .FirstOrDefault(inventorySlot => inventorySlot.ItemData.Id == item.Data.Id);
        
    }

    [Serializable]
    public class Container 
    {
        public Slot[] InventorySlots = new Slot[36];
    }

    [Serializable]
    public class Slot
    {
        public ItemCategory[] ItemCategories = Array.Empty<ItemCategory>();
        public ItemData ItemData;
        public int Amount;
        public bool CanBeChanged = true;
        public bool Discard;

        public Slot()
        {
            ItemData = null;
            Amount = 0;
        }

        public Slot(ItemData itemData, int amount)
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

        public void RemoveAmount(int value)
        {
            
        }
        
        public bool CanBeReplaced(Item item)
        {
            if (ItemCategories.Length <= 0 || item == null || item.Data.Id < 0)
            {
                return true;
            }

            if (Discard)
                return true;
            
            var anyItem = ItemCategories.Any(t => (t & item.Category) == t);

            return anyItem;
        }

        public void RemoveItem()
        {
            ItemData = new ItemData();
            Amount = 0;
        }
    }

    [Flags]
    public enum ItemCategory {
        Item = 1,
        Armor = Item | 1<< 2,
        Helmet = Item | 1 << 3,
        Chest = Item | 1 << 4,
        Pants = Item | 1 << 5,
        Consumable = Item | 1 << 6,
        Potion = Consumable | 1 << 7,
        Skill = 1<<8,
        Weapon = Item | 1 << 9,
        Bow = Weapon | 1<<10,
        Sword = Weapon | 1<<11,
        Stuff = Weapon | 1<<12,
    }
}