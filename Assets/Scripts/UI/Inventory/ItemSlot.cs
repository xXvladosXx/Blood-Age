using InventorySystem.Items;

namespace UI.Inventory
{
    using InventorySystem;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class ItemSlot : MonoBehaviour, IPointerClickHandler
    {
        private Item _item;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_item is InventoryItem inventoryItem)
            {
            }
        }

        public void SetItemData(Item item)
        {
            _item = item;
        }
    }
}