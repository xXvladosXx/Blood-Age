namespace DefaultNamespace.UI.Stats
{
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using Stats = DefaultNamespace.Stats;

    public class StatsPanel : MonoBehaviour
    {
        [SerializeField] private int _level;
        [SerializeField] private TextMeshProUGUI _levelUI;

        private StatsDisplay[] _statsDisplays;
        private Dictionary<Stats, float> _stats;

        private void Awake()
        {
            _statsDisplays = GetComponentsInChildren<StatsDisplay>();
        }

        private void Start()
        {
            foreach (var statsDisplay in _statsDisplays)
            {
                foreach (var stat in _stats)
                {
                    statsDisplay.SetStat(stat.Key, stat.Value);
                }
            }
        }

        public void SetStats(Dictionary<Stats, float> stats)
        {
            _stats = stats;
        }
    }
}