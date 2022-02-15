using System;
using SaveSystem;
using UnityEngine;

namespace StatsSystem
{
    public class Mana : ISavable
    {
        private float _currentMana;
        private float _maxMana;

        public event Action<float> OnManaPctChanged = delegate(float f) { };

        public float GetMaxMana => _maxMana;
        public float GetCurrentMana => _currentMana;

        public Mana(float maxMana)
        {
            _currentMana = maxMana;
            _maxMana = maxMana;
        }

        public void RenewStaminaPoints(float maxMana)
        {
            _maxMana = maxMana;
            _currentMana = maxMana;
        }

        public bool HasEnoughMana(float staminaPoints)
        {
            if (_currentMana - staminaPoints < 0) return false;

            _currentMana = Mathf.Clamp(_currentMana - staminaPoints, 0, _maxMana);
            float manaPtc = _currentMana / _maxMana;
            OnManaPctChanged?.Invoke(manaPtc);
            return true;
        }
        public void AddManaPoints(float manaPoints)
        {
            if(_currentMana == _maxMana) return;
            _currentMana = Mathf.Clamp(_currentMana + manaPoints, 0, _maxMana);

            float currentHealthPct = _currentMana / _maxMana;
            OnManaPctChanged?.Invoke(currentHealthPct);
        }
        public object CaptureState()
        {
            return _currentMana;
        }

        public void RestoreState(object state)
        {
            _currentMana = (float) state;
        }
    }
}