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
        
        private InventoryItem _inventoryItem;
        public event Action<InventoryItem> OnItemClicked;
        
        public void SetInventoryItem(InventoryItem inventoryItem, Slot slot)
        {
            _inventoryItem = inventoryItem;
            _image.sprite = inventoryItem.UIDisplay;
            _amount.text = inventoryItem.Stackable ? slot.Amount.ToString() : "";
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnItemClicked?.Invoke(_inventoryItem);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UnequippedItemTooltip.Instance.ShowTooltip(_inventoryItem);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UnequippedItemTooltip.Instance.HideTooltip();
        }
    }
}