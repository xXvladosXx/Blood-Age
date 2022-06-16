using System.Collections;
using Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Stats
{
    public class ManaBarPlayer : Panel
    {
        [SerializeField] private TextMeshProUGUI _manaValue;
        [SerializeField] private Image _image;
        [SerializeField] private float _updateSpeedSeconds = 0.2f;

        private AliveEntity _aliveEntity;
        private float _currentMana;
        public override void Initialize(AliveEntity aliveEntity)
        {
            _aliveEntity = aliveEntity;
            _currentMana = _aliveEntity.GetMana.GetCurrentMana;
        
            _aliveEntity.GetMana.OnManaPctChanged += OnManaChanged;
            _aliveEntity.OnCharacteristicChange += OnCharacteristicChange;
        
            _manaValue.text = $"{_currentMana:#} / {_aliveEntity.GetMana.GetMaxMana}";
        }

        private void OnEnable()
        {
            StartCoroutine(WaitSecondToRefreshHealth());
        }

        private IEnumerator WaitSecondToRefreshHealth()
        {
            yield return new WaitForSeconds(.1f);
            
            OnCharacteristicChange();
            
            _aliveEntity.GetMana.OnManaPctChanged += OnManaChanged;
            _aliveEntity.OnCharacteristicChange += OnCharacteristicChange;
        }
        private void OnCharacteristicChange()
        {
            _currentMana = _aliveEntity.GetMana.GetCurrentMana;
            StartCoroutine(ChangeToPct(_currentMana / _aliveEntity.GetMana.GetMaxMana));
            _manaValue.text = $"{_currentMana:#} / {_aliveEntity.GetMana.GetMaxMana}";
        }

        private void OnDisable()
        {
            _aliveEntity.GetMana.OnManaPctChanged -= OnManaChanged;
            _aliveEntity.OnCharacteristicChange -= OnCharacteristicChange;
        }

        private void OnManaChanged(float health)
        {
            _currentMana = _aliveEntity.GetMana.GetCurrentMana;
            StartCoroutine(ChangeToPct(health));
            
            _manaValue.text = $"{_currentMana:#} / {_aliveEntity.GetMana.GetMaxMana}";
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