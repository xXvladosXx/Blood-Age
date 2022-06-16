using InventorySystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Quests
{
    public class RewardDisplay : MonoBehaviour
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _amount;

        public void SetData(InventoryItem inventoryItem, int amount)
        {
            _itemImage.sprite = inventoryItem.UIDisplay;
            _amount.text = amount.ToString();
        }

        public void SetData(float experienceReward)
        {
            _amount.text = experienceReward.ToString();
        }
    }
}