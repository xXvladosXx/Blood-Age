using System.Globalization;
using DefaultNamespace;
using Entity;
using StatsSystem;
using TMPro;
using UnityEngine;

namespace UI.Stats
{
    public class StatsComparer : MonoBehaviour
    {
        [SerializeField] private Characteristics _characteristics;
        [SerializeField] private TextMeshProUGUI _characteristicName;
        [SerializeField] private TextMeshProUGUI _statMainValue;
        [SerializeField] private TextMeshProUGUI _statAdditionalValue;
        [SerializeField] private TextMeshProUGUI _describe;
        
        private void Start()
        {
            _characteristicName.text = _characteristics.ToString();
        }

        public void UpdateCharacteristics(AliveEntity aliveEntity)
        {
            _statMainValue.text = aliveEntity.GetStat(_characteristics).ToString(CultureInfo.InvariantCulture);
        }
    }
}