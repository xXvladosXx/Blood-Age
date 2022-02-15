using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using InventorySystem;
using InventorySystem.Items;
using UnityEngine;

namespace ShopSystem
{
    public class Seller : MonoBehaviour
    {
        [SerializeField] private InventoryContainer _sellerContainer;
        [SerializeField] private float _priceModifier;

        private Dictionary<InventoryItem, int> _transaction = new Dictionary<InventoryItem, int>();
        private Dictionary<InventoryItem, int> _stock = new Dictionary<InventoryItem, int>();
        private Customer _customer;
        private bool _buyingState;
        public event Action OnInventoryChange;

        public InventoryContainer GetInventoryContainer => _sellerContainer;

        public List<InventoryItem> GetFilteredItems(ItemCategory itemCategory)
        {
            List<InventoryItem> list = new List<InventoryItem>();
            foreach (var item in _sellerContainer.GetInventoryItems())
            {
                if ((item.Category & itemCategory) == itemCategory) list.Add(item as InventoryItem);
            }

            return list;
        }

        public void SellItem(InventoryItem inventoryItem, int amount)
        {
            _sellerContainer.AddItem(inventoryItem.Data, amount);
            OnInventoryChange?.Invoke();
        }
        
        public void ConfirmPurchase(InventoryItem inventoryItem, Customer customer, int amount)
        {
            if(!customer.BuyItem(inventoryItem, amount)) return;
            _sellerContainer.RemoveItem(inventoryItem.Data.Id, inventoryItem.Stackable ? amount : 0);
            OnInventoryChange?.Invoke();
        }

    }
}