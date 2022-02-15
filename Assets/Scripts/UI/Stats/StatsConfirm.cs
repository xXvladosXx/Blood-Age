using System;
using Entity;
using StatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Stats
{
    public class StatsConfirm : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Button _confirmButton;
    
        private StatsValueStore _statsValue;
        public event Action OnStatsConfirmed;

        private void CharacteristicUpdated()
        {
            if (_statsValue != null)
            {
                _statsValue.Confirm();
            }
            OnStatsConfirmed?.Invoke();
        }

        private void Update()
        {
            if(_statsValue == null) return;
            
            _valueText.text = _statsValue.GetUnassignedPoints.ToString();
        }

        public void SetInfo(AliveEntity aliveEntity)
        {
            _statsValue = aliveEntity.GetStatsValueStore;
            _confirmButton.onClick.AddListener(CharacteristicUpdated);
        }
    }
}
