using TMPro;
using UnityEngine;

namespace UI.Tooltip.Components
{
    public class ItemComparer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemStat;

        public void SetStat(string itemName, string itemStats)
        {
            _itemName.text = itemName;
            _itemStat.text = itemStats;
        }
    }
}