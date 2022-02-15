using System;
using InventorySystem;
using InventorySystem.Items;
using UnityEngine;

namespace ShopSystem
{
    public class Customer : MonoBehaviour
    {
        private ItemPicker _itemPicker;
        private ItemContainer _inventory;
        private int _gold;
        
        public event Action OnInventoryChange;
        public event Action<InventoryItem> OnItemBuy;

        public ItemContainer GetItemContainer => _inventory;
        
        private void Awake()
        {
            _itemPicker = GetComponent<ItemPicker>();
            _inventory = _itemPicker.GetItemContainer;
        }

        public int GetGold()
        {
            var gold = _inventory.FindSlotInInventory(new ItemData(15));
            if (gold == null)
                return 0;
            
            _gold = gold.Amount;

            return _gold;
        }

        public bool BuyItem(InventoryItem inventoryItem, int amount = 1)
        {
            if (!_inventory.AddItem(inventoryItem.Data, amount)) return false;
            var gold = inventoryItem.Price * amount;
            AddGold(gold, false);
           
            OnItemBuy?.Invoke(inventoryItem);
            OnInventoryChange?.Invoke();
            return true;
        }

        private void AddGold(int amount, bool add)
        {
            if (add)
            {
                _inventory.AddItem(new ItemData(15), amount);
            }
            else
            {
                _inventory.RemoveItem(15, amount);
            }
        }

        public void ConfirmSelling(InventoryItem itemToPurchase, Seller seller, int amount)
        {
            seller.SellItem(itemToPurchase, _inventory.GetSlotOfItem(itemToPurchase).Amount);
            var gold = itemToPurchase.Price * amount;
            AddGold(gold, true);
            _inventory.RemoveItem(itemToPurchase.Data.Id, itemToPurchase.Stackable ? amount : 0);
            OnInventoryChange?.Invoke();
        }
    }
}