using System;
using SaveSystem;
using UnityEngine;

namespace StatsSystem
{
    public class Stamina : ISavable
    {
        private int _delayTimeToRegenerateStamina = 3;
        
        private float _currentStamina;
        private float _maxStamina;
        private bool _canRegenerateStamina;

        private LTDescr _delay;

        public event Action<float> OnStaminaPctChanged = delegate(float f) { };

        public float GetMaxStamina => _maxStamina;
        public float GetCurrentStamina => _currentStamina;
        public Stamina()
        {
            _currentStamina = 100;
            _maxStamina = 100;
        }

        public void RenewStaminaPoints()
        {
            _maxStamina = 100;
            _currentStamina = 100;
        }

        public bool HasEnoughStamina(float staminaPoints)
        {
            if (_currentStamina - staminaPoints < 0) return false;

            _currentStamina = Mathf.Clamp(_currentStamina - staminaPoints, 0, _maxStamina);
            float staminaPct = _currentStamina / _maxStamina;
            StartDelay();
            OnStaminaPctChanged?.Invoke(staminaPct);
            return true;
        }

        public object CaptureState()
        {
            return _currentStamina;
        }

        public void RestoreState(object state)
        {
            _currentStamina = (float) state;
        }

        public void AddStaminaPoints(float staminaPoints)
        {
            if(!_canRegenerateStamina) return;
            if(_currentStamina == _maxStamina) return;
            _currentStamina = Mathf.Clamp(_currentStamina + staminaPoints, 0, _maxStamina);

            float currentStamina = _currentStamina / _maxStamina;
            OnStaminaPctChanged?.Invoke(currentStamina);
        }

        public void StartDelay()
        {
            _canRegenerateStamina = false;
            if(_delay!= null)
                LeanTween.cancel(_delay.uniqueId);

            _delay = LeanTween.delayedCall(_delayTimeToRegenerateStamina, () =>
            {
                _canRegenerateStamina = true;
            });
        }
    }
}