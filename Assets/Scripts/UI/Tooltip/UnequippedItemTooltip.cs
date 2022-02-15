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
        private List<ItemComparer> _itemComparers;

        public void ShowTooltip(Item overlappedInventoryItem)
        {
            _canvas.ForceUpdateRectTransforms();
            Update();

            if (overlappedInventoryItem == null) return;
            if (overlappedInventoryItem is InventoryItem inventoryItem)
            {
                _itemNameText.text = inventoryItem.name;
                _itemSlotText.text = inventoryItem.Category.ToString();
                _price.text = $"Price: {inventoryItem.Price.ToString()}";

                _stringBuilder.Length = 0;
                _stringBuilder.Append(inventoryItem.ItemInfo());

                _itemStatsText.text = _stringBuilder.ToString();

                gameObject.SetActive(true);
            }
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);

            foreach (var itemComparer in _itemComparers)
            {
                Destroy(itemComparer.gameObject);
            }

            _itemComparers.Clear();
        }

        protected override void Initialize()
        {
            Instance = this;
            _itemComparers = new List<ItemComparer>();

            HideTooltip();

            _stringBuilder = new StringBuilder();
        }
    }
}