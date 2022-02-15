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
    public class AdditionFilter : MonoBehaviour
    {
        [SerializeField] private List<AdditionalFilterButton> _additionFilters = new List<AdditionalFilterButton>();
        private Seller _seller;

        [Serializable]
        public class AdditionalFilterButton
        {
            public Button SubFilterButton;
            public ItemCategory ItemCategory;
        }
        
        public event Action<List<InventoryItem>> OnSubItemFilterChange;
        
        public void SetSeller(Seller seller)
        {
            _seller = seller;

            foreach (var additionFilter in _additionFilters)
            {
                additionFilter.SubFilterButton.onClick.AddListener(() =>
                {
                    OnSubItemFilterChange?.Invoke(_seller.GetFilteredItems(additionFilter.ItemCategory));
                    gameObject.SetActive(false);
                });
            }
        }

    }
}