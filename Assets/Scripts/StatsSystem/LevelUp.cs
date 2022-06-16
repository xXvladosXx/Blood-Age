using SaveSystem;
using UnityEngine;

namespace StatsSystem
{
    using System;

    [Serializable]
    public class LevelUp : ISavable, IRenewable
    {
        private float _currentExp;
        private float _maxExp;
        private FindStats _findStats;
        
        public event Action OnExperienceGive;
        public event Action<float> OnExperienceGivePct;
        public event Action<Class> OnEnemyDie;
        public event Action OnStatRenewed;

        public float GetCurrentExp => _currentExp;

        public LevelUp(FindStats findStats)
        {
            _findStats = findStats;
        }

        public void ExperienceReward(float experience)
        {
            _currentExp += experience;
            
            _maxExp = _findStats.GetStat(Characteristics.ExperienceToLevelUp);
            float currentExpPct = _currentExp /_maxExp;
            
            OnExperienceGive?.Invoke();
            OnExperienceGivePct?.Invoke(currentExpPct);
        }
        
        public void ExperienceReward(float experience, FindStats diedEnemy)
        {
            _currentExp += experience;

            _maxExp = _findStats.GetStat(Characteristics.ExperienceToLevelUp);
            float currentExpPct = _currentExp /_maxExp;
            
            OnExperienceGive?.Invoke();
            OnExperienceGivePct?.Invoke(currentExpPct);
            OnEnemyDie?.Invoke(diedEnemy.GetClass);
        }

        public object CaptureState()
        {
            return _currentExp;
        }

        public void RestoreState(object state)
        {
            _currentExp = (float) state;
        }


        public void Renew()
        {
            float currentExpPct = _currentExp /_maxExp;
            OnExperienceGive?.Invoke();
            OnExperienceGivePct?.Invoke(currentExpPct);
            OnStatRenewed?.Invoke();
        }
    }
}