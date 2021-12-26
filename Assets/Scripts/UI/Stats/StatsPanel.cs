namespace DefaultNamespace.UI.Stats
{
    using System;
    using System.Collections.Generic;
    using DefaultNamespace.Entity;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using Stats = DefaultNamespace.Stats;

    public class StatsPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private int _level;
        [SerializeField] private TextMeshProUGUI _levelUI;
        [SerializeField] private AliveEntity _aliveEntity;

        private bool _isOnInterface;
        private StatsConfirm _statsConfirm;

        public bool GetIsOnInterface => _isOnInterface;

        private void Awake()
        {
            _statsConfirm = GetComponentInChildren<StatsConfirm>();

            _statsConfirm.OnStatsConfirmed += StatsTooltip.Instance.ShowStatTooltip;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isOnInterface = true;
            StatsTooltip.Instance.ShowStatTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isOnInterface = false;
            StatsTooltip.Instance.HideStatTooltip();
        }
    }
}