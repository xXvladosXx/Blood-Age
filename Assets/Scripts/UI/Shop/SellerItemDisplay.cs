using System;
using InventorySystem;
using InventorySystem.Items;
using TMPro;
using UI.Tooltip;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Shop
{
    public class SellerItemDisplay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _amount;

        private int _priceModifier;
        private InventoryItem _inventoryItem;
        private Slot _slot;
        public event Action<Slot> OnItemClicked;
        
        public void SetInventoryItem(InventoryItem inventoryItem, Slot slot, int priceModifier = 1)
        {
            _inventoryItem = inventoryItem;
            _slot = slot;
            _priceModifier = priceModifier;

            _image.sprite = inventoryItem.UIDisplay;
            _amount.text = inventoryItem.Stackable ? slot.Amount.ToString() : "";
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemClicked?.Invoke(_slot);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UnequippedItemTooltip.Instance.ShowTooltip(_inventoryItem, _priceModifier);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UnequippedItemTooltip.Instance.HideTooltip();
        }
    }
}