using System.Collections.Generic;
using DG.Tweening;
using InventorySystem.Items;
using UI.Tooltip.Components;

namespace UI.Tooltip
{
    using System.Text;
    using InventorySystem;
    using TMPro;
    using UnityEngine;

    public class EquippedItemTooltip : DynamicTooltip
    {
        public static EquippedItemTooltip Instance;

        [SerializeField] private EquippedItemComparer _equippedItemComparer;

        private List<EquippedItemComparer> _equippedItemComparers = new List<EquippedItemComparer>();

        protected override void Initialize()
        {
            Instance = this;

            HideTooltip();
        }

        public void ShowTooltip(Item overlappedInventoryItem)
        {
            if (overlappedInventoryItem is InventoryItem inventoryItem)
            {
                var itemComparer = Instantiate(_equippedItemComparer, transform);
                _equippedItemComparers.Add(itemComparer);
                itemComparer.SetData(inventoryItem);

                Update();
                gameObject.SetActive(true);
            }
        }


        public void HideTooltip()
        {
            gameObject.SetActive(false);

            if (_equippedItemComparers == null) return;
            foreach (var itemComparer in _equippedItemComparers)
            {
                itemComparer.DestroyElement();
            }

            _equippedItemComparers.Clear();
        }
    }
}