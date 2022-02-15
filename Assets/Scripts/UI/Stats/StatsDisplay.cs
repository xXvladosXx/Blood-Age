using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI.Stats
{
    public class StatsDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private StatsSystem.Stats _stat;

        private void Awake()
        {
            _nameText.text = _stat.ToString();
        }

        public void SetStat(StatsSystem.Stats stat, float value)
        {
            if (_stat == stat)
            {
                _valueText.text = value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
