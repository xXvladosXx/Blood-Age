using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using InventorySystem;
using InventorySystem.Items;
using ShopSystem;
using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Shop
{
    public class SellerPanel : Panel, IPointerClickHandler
    {
        public static SellerPanel Instance { get; private set; }
        
        [SerializeField] private Transform _content;
        [SerializeField] private SellerItemDisplay _sellerItemDisplay;
        [SerializeField] private TransactionConfirmation _buyingConfirmation;
        
        private List<AdditionFilter> _additionFilters = new List<AdditionFilter>();
        private List<ItemFilter> _itemFilters = new List<ItemFilter>();

        private ItemFilter _currentItemFilter;

        private Dictionary<SellerItemDisplay, InventoryItem> _sellerItemsDisplay =
            new Dictionary<SellerItemDisplay, InventoryItem>();

        private InventoryContainer _inventoryContainer;
        private Seller _seller;
        private Customer _customer;
        
        public override void Initialize(AliveEntity aliveEntity)
        {
            Instance = this;
            
            _additionFilters = GetComponentsInChildren<AdditionFilter>().ToList();
            _itemFilters = GetComponentsInChildren<ItemFilter>().ToList();
            _buyingConfirmation = GetComponentInChildren<TransactionConfirmation>();
            _customer = aliveEntity.GetComponent<Customer>();

            _buyingConfirmation.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        private void RefreshShop()
        {
            DisplaySellerInventory(_inventoryContainer);
        }

        private void OnEnable()
        {
            ChangeUI(this);

            foreach (var itemFilter in _itemFilters)
            {
                itemFilter.SetSeller(_seller);
                itemFilter.OnItemFilterChange += DisplayFilteredItems;
            }
            
            foreach (var additionFilter in _additionFilters)
            {
                additionFilter.SetSeller(_seller);
                additionFilter.OnSubItemFilterChange += DisplayFilteredItems;
            }
        }

        private void DisplayFilteredItems(List<InventoryItem> filteredInventoryItems)
        {
            DisplayItems(filteredInventoryItems);
        }

        private void DisplaySellerInventory(InventoryContainer inventoryContainer)
        {
            _inventoryContainer = inventoryContainer;
            
            var inventoryItems = _inventoryContainer.GetInventoryItems()
                .Select(item => item as InventoryItem).ToList();
            
            DisplayItems(inventoryItems);
            
            foreach (var itemFilter in _itemFilters)
            {
                itemFilter.OnItemFilterEnter += OnItemFilterEnter;
            }

            foreach (var additionFilter in _additionFilters)
            {
                additionFilter.gameObject.SetActive(false);
            }
        }

        private void DisplayItems(List<InventoryItem> inventoryItems)
        {
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

        private void ConfirmPurchase(InventoryItem itemToPurchase)
        {
            _buyingConfirmation.SetItemToPurchase(itemToPurchase, _customer, _seller, _inventoryContainer.FindSlotInInventory(itemToPurchase.Data).Amount);
            _buyingConfirmation.gameObject.SetActive(true);
        }

        private void OnItemFilterEnter(ItemFilter hoveredItemFilter)
        {
            if (hoveredItemFilter != _currentItemFilter)
            {
                DisableAdditionalFilter();    
            }
            
            _currentItemFilter = hoveredItemFilter;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            DisableAdditionalFilter();
        }

        private void DisableAdditionalFilter()
        {
            foreach (var additionFilter in _additionFilters)
            {
                additionFilter.gameObject.SetActive(false);
            }
        }

        public void ShowSellerInventory(InventoryContainer sellerInventory, Seller seller)
        {
            _seller = seller;
            _seller.OnInventoryChange += RefreshShop;
            DisplaySellerInventory(sellerInventory);
            gameObject.SetActive(true);
        }
    }

}