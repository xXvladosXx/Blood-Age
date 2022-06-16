using Entity;
using TMPro;
using UI.Stats;
using UI.Tooltip.Components;
using UnityEngine;

namespace UI.Tooltip
{
    public class StatsTooltip : Panel
    {
        public static StatsTooltip Instance { get; private set; }

        [SerializeField] private RectTransform _canvas;
        [SerializeField] private RectTransform _background;
        [SerializeField] private ItemComparer _itemComparer;
        [SerializeField] private TextMeshProUGUI _stats;
        

        public void ShowStatTooltip(string s)
        {
            _stats.text = s;
            
            gameObject.SetActive(true);
        }

        public void HideStatTooltip()
        {
            gameObject.SetActive(false);
        }

        public override void Initialize(AliveEntity aliveEntity)
        {
            Instance = this;

            HideStatTooltip();
        }
    }
}