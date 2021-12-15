namespace DefaultNamespace.UI.Stats
{
    using System.Text;
    using TMPro;
    using UnityEngine;

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