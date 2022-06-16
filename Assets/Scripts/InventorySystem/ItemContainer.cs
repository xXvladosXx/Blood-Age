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

        public event Action OnInventoryChange;

        protected abstract void RegisterFilters();

        private void OnValidate()
        {
            RegisterFilters();
        }

        private bool HasEnough() => HasEmptySlot > 0;

        public bool AddItem(ItemData itemData, int amount)
        {
            var inventorySlot = FindSlotInInventory(itemData);

            if ((Database.Items[itemData.Id].Stackable && inventorySlot != null))
            {
                inventorySlot.AddAmount(amount);
                return true;
            }

            if (!HasEnough()) return false;
            if (!Filters.Select(f => f(itemData, amount)).All(e => e)) return false;

            SetSlot(itemData, amount);
            OnInventoryChange?.Invoke();
            return true;
        }

        public bool SwapItem(ItemContainer uiItemContainer, Slot draggedItem, Slot replacedItem)
        {
            if (!draggedItem.CanBeReplaced(Database.GetItemByID(replacedItem.ItemData.Id))) return false;
            if (!replacedItem.CanBeReplaced(Database.GetItemByID(draggedItem.ItemData.Id))) return false;

            var temp = new Slot(replacedItem.ItemData, replacedItem.Amount);
            replacedItem.UpdateSlot(draggedItem.ItemData, draggedItem.Amount);

            draggedItem.UpdateSlot(temp.ItemData, temp.Amount);

            OnInventoryChange?.Invoke();
            uiItemContainer.OnInventoryChange?.Invoke();

            return true;
        }
        
        public void SwapDragged(ItemContainer uiItemContainer, Slot draggedItem, Slot replacedItem)
        {
            var temp = new Slot(replacedItem.ItemData, replacedItem.Amount);

            draggedItem.UpdateSlot(temp.ItemData, temp.Amount);

            OnInventoryChange?.Invoke();
            uiItemContainer.OnInventoryChange?.Invoke();
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
                        if (inventorySlot.Amount <= 0 && Database.GetItemByID(id).Stackable)
                            inventorySlot.UpdateSlot(new ItemData(), 0);

                        break;
                    }
                }
            }

            OnInventoryChange?.Invoke();
        }

        public List<Slot> GetAllSlots()
        {
            return Container.InventorySlots.ToList();
        }

        public List<int> GetAllItems()
        {
            List<int> items = new List<int>(Container.InventorySlots.Length);

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
            {
                var item = Database.GetItemByID(inventorySlot.ItemData.Id);
                if (item != null)
                    list.Add(item);
            }

            return list;
        }

        private Slot SetSlot(ItemData itemData, int amount)
        {
            foreach (var inventorySlot in Container.InventorySlots)
            {
                if (inventorySlot.ItemData.Id > -1 ||
                    !inventorySlot.CanBeReplaced(Database.GetItemByID(itemData.Id))) continue;

                inventorySlot.UpdateSlot(itemData, amount);

                return inventorySlot;
            }

            return null;
        }

        public Item GetItemOnIndexSlot(int index)
        {
            return Database.GetItemByID(Container.InventorySlots[index].ItemData.Id);
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

        public bool CanBeReplaced(Item item)
        {
            if (ItemCategories.Length <= 0 || item == null || item.Data.Id < 0)
            {
                return true;
            }

            if (Discard)
            {
                return true;
            }

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
    public enum ItemCategory
    {
        Item = 1,
        Armor = Item | 1 << 2,
        Helmet = Item | 1 << 3,
        Chest = Item | 1 << 4,
        Pants = Item | 1 << 5,
        Consumable = Item | 1 << 6,
        Potion = Consumable | 1 << 7,
        Skill = 1 << 8,
        Weapon = Item | 1 << 9,
        Bow = Weapon | 1 << 10,
        Sword = Weapon | 1 << 11,
        Stuff = Weapon | 1 << 12,
        Gloves = Item | 1 << 13,
        Shoulders = Item | 1 << 14,
        Boots = Item | 1 << 15,
        Health = Potion | 1 << 16,
        Mana = Potion | 1 << 17,
    }
}