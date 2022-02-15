using System;
using System.Collections.Generic;
using System.Linq;
using AttackSystem.AttackMechanics;
using Entity;
using SaveSystem;
using UnityEngine;
using Random = System.Random;

namespace StatsSystem
{
    [Serializable]
    public class Health : ISavable
    {
        private float _currentHealth;
        private float _maxHealth;
        private bool _isDead;
        private FindStats _findStats;

        public event Action<AliveEntity> OnTakeHit;
        public event Action<float> OnHealthPctChanged = delegate(float f) { };
        public event Action OnDie;
        public event Action OnBloodyDie;

        public float GetMaxHealth => _maxHealth;
        public float GetCurrentHealth => _currentHealth;

        public FindStats GetUser => _findStats;

        public bool IsDead() => _isDead;

        public Health(FindStats findStats)
        {
            _currentHealth = findStats.GetStat(Characteristics.Health);
            _maxHealth = findStats.GetStat(Characteristics.Health);
            _findStats = findStats;

            if (findStats.GetComponent<StarterAssetsInputs>() != null) return;
        }

        public void RenewHealthPoints(float healthPoints)
        {
            _maxHealth = healthPoints;
        }

        public void TakeHit(AttackData attackData)
        {
            if(_isDead) return;
            
            if (CheckEnemiesList(attackData)) return;
            if (CalculateChanceOfHit(attackData)) return;
            CalculateCriticalHit(attackData);
            CalculateElementDamage(attackData);
            
            _currentHealth = Mathf.Clamp(_currentHealth - attackData.Damage, 0, _currentHealth);

            float currentHealthPct = _currentHealth / _maxHealth;
            
            OnHealthPctChanged?.Invoke(currentHealthPct);
            OnTakeHit?.Invoke(attackData.Damager);
            Debug.Log("Damage " + attackData.Damage);
            
            if (_maxHealth < attackData.Damage)
            {
                OnBloodyDie?.Invoke();
            }
            
            if (_currentHealth <= 0)
            {
                attackData.Damager.GetLevelData.ExperienceReward(_findStats.GetStat(Characteristics.DeathExperience),
                    _findStats);
                OnDie?.Invoke();
                _isDead = true;
            }
        }

        private void CalculateCriticalHit(AttackData attackData)
        {
            Random random = new Random();
            var num = random.NextDouble() * 100;
            Debug.Log(attackData.Damage);
            Debug.Log(attackData.CriticalDamage);
            if (num < attackData.CriticalChance)
            {
                var startDamage =  attackData.Damage;
                attackData.Damage += attackData.Damage * (attackData.CriticalDamage/100);
                attackData.Damage -= startDamage;
            }
        }

        private static bool CheckEnemiesList(AttackData attackData)
        {
            if (attackData.Entities != null)
            {
                if (attackData.Entities.Any(entity => !attackData.Targets.Contains(entity)))
                {
                    return true;
                }
            }

            return false;
        }

        private static void CalculateElementDamage(AttackData attackData)
        {
            var resistance = attackData.Damager.GetItemEquipper.GetDamageResistance;

            float damage = 0;
            if (attackData.ElementalDamage != null)
            {
                foreach (var damagePair in attackData.ElementalDamage)
                {
                    damage += resistance.CalculateResistance(damagePair.Value, damagePair.Key);
                }
            }

            attackData.Damage += damage;
        }

        private bool CalculateChanceOfHit(AttackData attackData)
        {
            Random random = new Random();
            var num = random.NextDouble() * 100;
            var limit = (attackData.Accuracy + _findStats.GetStat(Characteristics.Evasion))
                        * (100 - _findStats.GetStat(Characteristics.Evasion));

            if (num > limit) return true;
            return false;
        }

        public object CaptureState()
        {
            return _currentHealth;
        }

        public void RestoreState(object state)
        {
            _currentHealth = (float) state;
        }

        public void AddHealthPoints(float healthReg)
        {
            if(_currentHealth == _maxHealth) return;
            _currentHealth = Mathf.Clamp(_currentHealth + healthReg, 0, _maxHealth);

            float currentHealthPct = _currentHealth / _maxHealth;
            OnHealthPctChanged?.Invoke(currentHealthPct);
        }
    }
}