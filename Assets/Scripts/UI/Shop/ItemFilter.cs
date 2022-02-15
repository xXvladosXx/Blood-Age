using System;
using System.Collections.Generic;
using InventorySystem;
using InventorySystem.Items;
using ShopSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ItemFilter : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private AdditionFilter _additionalFiler;
        [SerializeField] private ItemCategory _itemCategory;

        private Button _button;
        private Seller _seller;

        public event Action<ItemFilter> OnItemFilterEnter;
        public event Action<List<InventoryItem>> OnItemFilterChange;
        
        public void SetSeller(Seller seller)
        {
            _seller = seller;
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(EnableFilterWithButton);
        }

        private void EnableFilterWithButton()
        {
            EnableAdditionalFilter();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void EnableAdditionalFilter()
        {
            if(_additionalFiler != null)
                _additionalFiler.gameObject.SetActive(true);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnItemFilterEnter?.Invoke(this);
            EnableAdditionalFilter();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemFilterChange?.Invoke(_seller.GetFilteredItems(_itemCategory));
        }
    }
}