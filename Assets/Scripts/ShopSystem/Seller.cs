using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using InventorySystem;
using InventorySystem.Items;
using SaveSystem;
using SceneSystem;
using UnityEngine;

namespace ShopSystem
{
    public class Seller : MonoBehaviour, ISavable
    {
        [SerializeField] private InventoryContainer _sellerContainer;
        [SerializeField] private int _priceModifier;

        private bool _buyingState;
        public event Action OnInventoryChange;
        public event Action OnShopClose;

        public InventoryContainer GetInventoryContainer => _sellerContainer;
        public int GetPriceModifier => _priceModifier;

        public List<InventoryItem> GetFilteredItems(ItemCategory itemCategory)
        {
            List<InventoryItem> list = new List<InventoryItem>();
            foreach (var item in _sellerContainer.GetInventoryItems())
            {
                if ((item.Category & itemCategory) == itemCategory) list.Add(item as InventoryItem);
            }

            return list;
        }

        public void CloseShop()
        {
            OnShopClose?.Invoke();
        }
        
        public void BuyItem(InventoryItem inventoryItem, int amount)
        {
            _sellerContainer.AddItem(inventoryItem.Data, amount);
            OnInventoryChange?.Invoke();
        }
        
        public void ConfirmSelling(InventoryItem inventoryItem, Customer customer, int amount)
        {
            if(!customer.CanBuyItem(inventoryItem, _priceModifier, amount)) return;
            
            _sellerContainer.RemoveItem(inventoryItem.Data.Id, inventoryItem.Stackable ? amount : 0);
            OnInventoryChange?.Invoke();
        }

        public object CaptureState()
        {
            var items = _sellerContainer.GetAllSlots();
            return items;
        }

        public void RestoreState(object state)
        {
            var items = state as List<Slot>;
            _sellerContainer.ClearInventory();
            foreach (var item in items)
            {
                var sellingItem = _sellerContainer.Database.GetItemByID(item.ItemData.Id);
                if (sellingItem != null)
                {
                    _sellerContainer.AddItem(sellingItem.Data, item.Amount);
                }
            }
        }
    }
}