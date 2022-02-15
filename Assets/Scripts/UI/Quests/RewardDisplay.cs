using InventorySystem;
using InventorySystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{
    public class RewardDisplay : MonoBehaviour
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _amount;

        public void SetData(InventoryItem inventoryItem, int amount)
        {
            
        }
    }
}