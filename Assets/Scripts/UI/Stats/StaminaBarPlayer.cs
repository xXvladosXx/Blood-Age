using System.Collections;
using Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Stats
{
    public class StaminaBarPlayer : Panel
    {
        [SerializeField] private TextMeshProUGUI _staminaValue;
        [SerializeField] private Image _image;
        [SerializeField] private float _updateSpeedSeconds = 0.2f;

        private AliveEntity _aliveEntity;
        private float _currentStamina;
        public override void Initialize(AliveEntity aliveEntity)
        {
            _aliveEntity = aliveEntity;
            _currentStamina = _aliveEntity.GetStamina.GetCurrentStamina;
        
            _aliveEntity.GetStamina.OnStaminaPctChanged += OnStaminaChanged;
            _aliveEntity.OnCharacteristicChange += OnCharacteristicChange;
        
            _staminaValue.text = $"{_currentStamina:#} / {_aliveEntity.GetStamina.GetMaxStamina}";
        }

        private void OnCharacteristicChange()
        {
            _currentStamina = _aliveEntity.GetStamina.GetCurrentStamina;
            StartCoroutine(ChangeToPct(_currentStamina / _aliveEntity.GetStamina.GetMaxStamina));
            _staminaValue.text = $"{_currentStamina:#} / {_aliveEntity.GetStamina.GetMaxStamina}";
        }

        private void OnDisable()
        {
            _aliveEntity.GetStamina.OnStaminaPctChanged -= OnStaminaChanged;
            _aliveEntity.OnCharacteristicChange -= OnCharacteristicChange;
        }

        private void OnStaminaChanged(float stamina)
        {
            _currentStamina = _aliveEntity.GetStamina.GetCurrentStamina;
            StartCoroutine(ChangeToPct(stamina));
            
            _staminaValue.text = $"{_currentStamina:#} / {_aliveEntity.GetStamina.GetMaxStamina}";
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