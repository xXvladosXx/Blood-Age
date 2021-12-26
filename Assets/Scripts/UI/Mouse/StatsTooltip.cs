namespace DefaultNamespace.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using DefaultNamespace.UI.Stats;
    using TMPro;
    using UnityEngine;

    public class StatsTooltip : MonoBehaviour
    {
        public static StatsTooltip Instance; 
        
        [SerializeField] private RectTransform _canvas;
        [SerializeField] private RectTransform _background;
        [SerializeField] private ItemComparer _itemComparer;
        [SerializeField] private AliveEntity _aliveEntity;
        [SerializeField] private StatsComparer[] _statsComparers;

        private void Awake()
        {
            Instance = this;
            HideStatTooltip();
            
            _statsComparers = GetComponentsInChildren<StatsComparer>();
        }
        
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
    }
}