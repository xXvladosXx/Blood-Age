using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void FindAddedStats()
        {
             StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<color=#FFFFFF>").Append("Health: ").Append(_aliveEntity.GetStat(Characteristics.Health)).Append("</color>").AppendLine();
            stringBuilder.Append("<color=#FFFFFF>").Append("Health Regeneration: ").Append(
                $"{_aliveEntity.GetStat(Characteristics.HealthRegeneration):0.00}").Append("</color>").AppendLine();

            stringBuilder.Append("<color=#FFFFFF>").Append("Mana: ").Append(_aliveEntity.GetStat(Characteristics.Mana)).Append("</color>").AppendLine();
            stringBuilder.Append("<color=#FFFFFF>").Append("Mana Regeneration: ").Append(
                $"{_aliveEntity.GetStat(Characteristics.ManaRegeneration):0.00}").Append("</color>").AppendLine();

            stringBuilder.Append("<color=#FFFFFF>").Append("Damage: ").Append(_aliveEntity.GetStat(Characteristics.Damage)).Append("</color>").AppendLine();
            stringBuilder.Append("<color=#FFFFFF>").Append("Critical chance: ").Append(_aliveEntity.GetStat(Characteristics.CriticalChance)).Append("</color>").AppendLine();
            stringBuilder.Append("<color=#FFFFFF>").Append("Critical Damage: ").Append(_aliveEntity.GetStat(Characteristics.CriticalDamage)).Append("</color>").AppendLine();
            stringBuilder.Append("<color=#FFFFFF>").Append("Evasion: ").Append(_aliveEntity.GetStat(Characteristics.Evasion)).Append("</color>").AppendLine();
            stringBuilder.Append("<color=#FFFFFF>").Append("Accuracy: ").Append(_aliveEntity.GetStat(Characteristics.Accuracy)).Append("</color>").AppendLine();

            stringBuilder.AppendLine().AppendLine();
            
            stringBuilder.Append("<color=#FFFFFF>").Append("Experience: ")
                .Append(_aliveEntity.GetLevelData.GetCurrentExp)
                .Append("/")
                .Append(_aliveEntity.GetStat(Characteristics.ExperienceToLevelUp))
                .Append("</color>").AppendLine();
            
            var s = stringBuilder.ToString();

            StatsTooltip.Instance.ShowStatTooltip(s);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            FindAddedStats();
        }
            
        public void OnPointerExit(PointerEventData eventData)
        {
            StatsTooltip.Instance.HideStatTooltip();
        }

        private void OnDisable()
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
            _statsConfirm.OnStatsConfirmed += FindAddedStats;
            _statsConfirm.SetInfo(_aliveEntity);
            
            _aliveEntity.GetFindStats.OnLevelRestored += () => _levelUI.text = _aliveEntity.GetLevel.ToString();
        }

    }
}