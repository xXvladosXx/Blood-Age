using Entity;
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
        [SerializeField] private StatsComparer[] _statsComparers;

        private AliveEntity _aliveEntity;

        public void ShowStatTooltip()
        {
            foreach (var statsComparer in _statsComparers)
            {
                statsComparer.UpdateCharacteristics(_aliveEntity);
            }

            gameObject.SetActive(true);
        }

        public void HideStatTooltip()
        {
            gameObject.SetActive(false);
        }

        public override void Initialize(AliveEntity aliveEntity)
        {
            _aliveEntity = aliveEntity;
            Instance = this;

            _statsComparers = GetComponentsInChildren<StatsComparer>();
            HideStatTooltip();
        }
    }
}