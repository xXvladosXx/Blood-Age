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
            _customer = aliveEntity.GetComponent<Customer>();
            _inventoryContainer = _customer.GetItemContainer;
            
            _inventoryContainer.OnInventoryChange += DisplayItems;
            _customer.OnInventoryShopOpen += SetSeller;

            _sellingConfirmation = GetComponentInChildren<SellingConfirmation>();
            
            _sellingConfirmation.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void DisplayItems()
        {
            var inventoryItems = _inventoryContainer.GetInventoryItems()
                .Select(item => item as InventoryItem).ToList();
            
            foreach (var key in _sellerItemsDisplay.Keys)
            {
                if(key != null)
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

        private void ConfirmPurchase(Slot itemToSell)
        {
            _sellingConfirmation.SetItemToPurchase(_inventoryContainer.Database.GetItemByID(itemToSell.ItemData.Id) as InventoryItem, _customer, _seller, itemToSell.Amount);
            _sellingConfirmation.gameObject.SetActive(true);
        }

        private void SetSeller(Seller seller)
        {
            _seller = seller;

            DisplayItems();
        }
    }
}