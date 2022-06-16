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

        public event Action<InventoryItem, int> OnItemBuy;
        public event Action<Seller> OnInventoryShopOpen;

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

        public bool CanBuyItem(InventoryItem inventoryItem, int priceModifier, int amount = 1)
        {
            if (!_inventory.AddItem(inventoryItem.Data, amount)) return false;
            var startPrice = inventoryItem.Price * amount;
            startPrice += (startPrice * priceModifier) / 100;
            RemoveGold(startPrice);

            OnItemBuy?.Invoke(inventoryItem, amount);
            return true;
        }

        private void AddGold(int amount)
        {
            _inventory.AddItem(new ItemData(15), amount);
        }

        private void RemoveGold(int amount)
        {
            _inventory.RemoveItem(15, amount);
        }

        public void ConfirmPurchase(InventoryItem itemToPurchase, Seller seller, int amount)
        {
            seller.BuyItem(itemToPurchase, amount);
            var gold = itemToPurchase.Price * amount;
            AddGold(gold);
            _inventory.RemoveItem(itemToPurchase.Data.Id, itemToPurchase.Stackable ? amount : 0);
        }

        public void OpenInventory(Seller seller)
        {
            OnInventoryShopOpen?.Invoke(seller);
        }
    }
}