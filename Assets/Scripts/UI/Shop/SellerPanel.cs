using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using InventorySystem;
using InventorySystem.Items;
using ShopSystem;
using TMPro;
using UI.Inventory;
using UI.Tooltip;
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
        [SerializeField] private TextMeshProUGUI _priceModifier;
        
        private List<AdditionFilter> _additionFilters = new List<AdditionFilter>();
        private List<ItemFilter> _itemFilters = new List<ItemFilter>();

        private ItemFilter _currentItemFilter;

        private Dictionary<SellerItemDisplay, InventoryItem> _sellerItemsDisplay =
            new Dictionary<SellerItemDisplay, InventoryItem>();
        private List<InventoryItem> _lastFilteredInventoryItems = new List<InventoryItem>();

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
            _lastFilteredInventoryItems = filteredInventoryItems;
        }

        private void DisplaySellerInventory(InventoryContainer inventoryContainer)
        {
            _priceModifier.text = $"{_seller.GetPriceModifier}%";
            _inventoryContainer = inventoryContainer;

            var inventoryItems = _lastFilteredInventoryItems;
            
            if (inventoryItems.Count == 0)
            {
                inventoryItems = _inventoryContainer.GetInventoryItems()
                    .Select(item => item as InventoryItem).ToList();
            }
            
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
                    _inventoryContainer.FindSlotInInventory(inventoryItem.Data), _seller.GetPriceModifier);
                _sellerItemsDisplay.Add(sellerItemDisplay, inventoryItem);
                sellerItemDisplay.OnItemClicked += ConfirmPurchase;
            }
        }

        private void ConfirmPurchase(Slot itemToPurchase)
        {
            _buyingConfirmation.SetItemToPurchase(_inventoryContainer.Database.GetItemByID(itemToPurchase.ItemData.Id) as InventoryItem, _customer, _seller, itemToPurchase.Amount);
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

        public void CloseShop()
        {
            _lastFilteredInventoryItems.Clear();
            _seller.CloseShop();
            ChangeUI(this);
        }
    }

}