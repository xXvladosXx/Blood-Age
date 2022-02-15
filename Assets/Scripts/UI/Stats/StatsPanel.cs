using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using StatsSystem;
using TMPro;
using UI.Inventory;
using UI.Tooltip;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Stats
{
    public class StatsPanel : Panel, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private int _level;
        [SerializeField] private TextMeshProUGUI _levelUI;
        [SerializeField] private TextMeshProUGUI _characterClass;

        private StatsConfirm _statsConfirm;
        private List<StatsDistributor> _statsDistributors;
        private AliveEntity _aliveEntity;

        public void OnPointerEnter(PointerEventData eventData)
        {
            StatsTooltip.Instance.ShowStatTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StatsTooltip.Instance.HideStatTooltip();
        }

        public override void Initialize(AliveEntity aliveEntity)
        {
            _aliveEntity = aliveEntity;
            _characterClass.text = _aliveEntity.SerializableClass.ToString();
            
            _levelUI.text = _aliveEntity.GetLevel.ToString();
            _statsDistributors = GetComponentsInChildren<StatsDistributor>().ToList();
            
            foreach (var statsDistributor in _statsDistributors)
            {
                statsDistributor.SetInfo(_aliveEntity);
            }
            
            _aliveEntity.GetLevelData.OnExperienceGive += () => _levelUI.text = _aliveEntity.GetLevel.ToString();

            _statsConfirm = GetComponentInChildren<StatsConfirm>();
            _statsConfirm.SetInfo(_aliveEntity);
        }

        private void OnDisable()
        {
            StatsTooltip.Instance.HideStatTooltip();
        }
    }
}