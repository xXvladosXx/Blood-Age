using System.Collections.Generic;
using System.Linq;
using Entity;
using InventorySystem;
using InventorySystem.Items;
using ShopSystem;
using UnityEngine;

namespace UI.Shop
{
    public class CustomerPanel : Panel
    {
        public static CustomerPanel Instance { get; private set; }

        [SerializeField] private Transform _content;
        [SerializeField] private SellerItemDisplay _sellerItemDisplay;
        [SerializeField] private TransactionConfirmation _sellingConfirmation;
        
        private ItemPicker _itemPicker;
        private Dictionary<SellerItemDisplay, InventoryItem> _sellerItemsDisplay =
            new Dictionary<SellerItemDisplay, InventoryItem>();

        private Customer _customer;
        private Seller _seller;
        private ItemContainer _inventoryContainer;

        public override void Initialize(AliveEntity aliveEntity)
        {
            Instance = this;
            _customer = aliveEntity.GetComponent<Customer>();
            _customer.OnInventoryChange += RefreshInventory;
            _inventoryContainer = _customer.GetItemContainer;
            _sellingConfirmation = GetComponentInChildren<SellingConfirmation>();
            
            _sellingConfirmation.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void RefreshInventory()
        {
            DisplayItems();
        }

        private void DisplayItems()
        {
            var inventoryItems = _inventoryContainer.GetInventoryItems()
                .Select(item => item as InventoryItem).ToList();
            
            foreach (var key in _sellerItemsDisplay.Keys)
            {
                Destroy(key.gameObject);
            }
            
            _sellerItemsDisplay.Clear();
            
            foreach (var inventoryItem in inventoryItems)
            {
                var sellerItemDisplay = Instantiate(_sellerItemDisplay, _content);
                sellerItemDisplay.SetInventoryItem(inventoryItem,
                    _inventoryContainer.FindSlotInInventory(inventoryItem.Data));
                _sellerItemsDisplay.Add(sellerItemDisplay, inventoryItem);
                sellerItemDisplay.OnItemClicked += ConfirmPurchase;
            }
        }

        private void ConfirmPurchase(InventoryItem itemToSell)
        {
            _sellingConfirmation.SetItemToPurchase(itemToSell, _customer, _seller, _inventoryContainer.FindSlotInInventory(itemToSell.Data).Amount);
            _sellingConfirmation.gameObject.SetActive(true);
        }

        public void SetSeller(Seller seller)
        {
            _seller = seller;
            DisplayItems();
        }
    }
}