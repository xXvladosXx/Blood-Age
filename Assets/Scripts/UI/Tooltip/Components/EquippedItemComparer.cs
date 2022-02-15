using System.Text;
using InventorySystem;
using InventorySystem.Items;
using TMPro;
using UnityEngine;

namespace UI.Tooltip.Components
{
    public class EquippedItemComparer: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _itemSlotText;
        [SerializeField] private TextMeshProUGUI _itemStatsText;
        [SerializeField] private TextMeshProUGUI _price;
        private StringBuilder _stringBuilder;

        public void SetData(InventoryItem inventoryItem)
        {
            _stringBuilder = new StringBuilder();
            
            _itemNameText.text = inventoryItem.name;
            _itemSlotText.text = inventoryItem.Category.ToString();
            _price.text = $"Price: {inventoryItem.Price.ToString()}" ;
            _stringBuilder.Length = 0;
            _stringBuilder.Append(inventoryItem.ItemInfo());

            _itemStatsText.text = _stringBuilder.ToString();
            
            gameObject.SetActive(true);
        }

        public void DestroyElement()
        {
            Destroy(gameObject);
        }
    }
}