using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using InventorySystem;
using InventorySystem.Items;
using TMPro;
using UI.Tooltip.Components;
using UnityEngine;

namespace UI.Tooltip
{
    public class UnequippedItemTooltip : DynamicTooltip
    {
        public static UnequippedItemTooltip Instance { get; private set; }

        [SerializeField] private ItemComparer _itemComparer;
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _itemSlotText;
        [SerializeField] private TextMeshProUGUI _itemStatsText;
        [SerializeField] private TextMeshProUGUI _price;

        private StringBuilder _stringBuilder;

        public void ShowTooltip(Item overlappedInventoryItem, int priceModifier = 1)
        {
            _canvas.ForceUpdateRectTransforms();

            if (overlappedInventoryItem == null) return;
            if (overlappedInventoryItem is InventoryItem inventoryItem)
            {
                _itemNameText.text = inventoryItem.Data.Name;
                _itemNameText.color = inventoryItem.Rarity.GetColor;
                _itemSlotText.text = inventoryItem.Category.ToString();

                var itemPrice = inventoryItem.Price;
                if (priceModifier > 1)
                {
                    itemPrice += inventoryItem.Price * priceModifier / 100;
                }
                
                _price.text = $"Price: {itemPrice.ToString()}";

                _stringBuilder.Length = 0;
                _stringBuilder.Append(inventoryItem.ItemInfo());

                _itemStatsText.text = _stringBuilder.ToString();

                Update();
                gameObject.SetActive(true);
            }
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }

        protected override void Initialize()
        {
            Instance = this;
            HideTooltip();

            _stringBuilder = new StringBuilder();
        }
    }
}