using System;
using System.Collections;
using Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Stats
{
    public class HealthBarPlayer : Panel
    {
        [SerializeField] private TextMeshProUGUI _healthValue;
        [SerializeField] private Image _image;
        [SerializeField] private float _updateSpeedSeconds = 0.2f;

        private AliveEntity _aliveEntity;
        private float _currentHealth;
        public override void Initialize(AliveEntity aliveEntity)
        {
            _aliveEntity = aliveEntity;
            _currentHealth = _aliveEntity.GetHealth.GetCurrentHealth;
        
            _aliveEntity.GetHealth.OnHealthPctChanged += OnHealthChanged;
            _aliveEntity.OnCharacteristicChange += () => OnCharacteristicChange();
        
            _healthValue.text = $"{_currentHealth:#} / {_aliveEntity.GetHealth.GetMaxHealth}";
        }

        private string OnCharacteristicChange()
        {
            _currentHealth = _aliveEntity.GetHealth.GetCurrentHealth;
            StartCoroutine(ChangeToPct(_currentHealth / _aliveEntity.GetHealth.GetMaxHealth));
            return _healthValue.text = $"{_currentHealth:#} / {_aliveEntity.GetHealth.GetMaxHealth}";
        }

        private void OnDisable()
        {
            _aliveEntity.GetHealth.OnHealthPctChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(float health)
        {
            _currentHealth = _aliveEntity.GetHealth.GetCurrentHealth;
            StartCoroutine(ChangeToPct(health));
            
            _healthValue.text = $"{_currentHealth:#} / {_aliveEntity.GetHealth.GetMaxHealth}";
        }
        private IEnumerator ChangeToPct(float pct)
        {
            float preChangePct = _image.fillAmount;
            float elapsed = 0f;

            while (elapsed < _updateSpeedSeconds)
            {
                elapsed += Time.deltaTime;
                _image.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / _updateSpeedSeconds);
                yield return null;
            }

            _image.fillAmount = pct;
        }
    }
}
