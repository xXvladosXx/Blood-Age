using System.Globalization;
using DefaultNamespace;
using StatsSystem;
using TMPro;
using UnityEngine;

namespace UI.Tooltip
{
    public class BuffTooltip : DynamicTooltip
    {
        public static BuffTooltip Instance{ get; private set; }

        [SerializeField] private TextMeshProUGUI _characteristics;
        [SerializeField] private TextMeshProUGUI _bonusValue;
        protected override void Initialize()
        {
            Instance = this;

            HideBuffTooltip();
        }

        public void HideBuffTooltip()
        {
            gameObject.SetActive(false);
        }

        public void ShowBuffTooltip(Characteristics characteristic, float bonusValue)
        {
            gameObject.SetActive(true);
            
            _characteristics.text = characteristic.ToString();
            _bonusValue.text = bonusValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}