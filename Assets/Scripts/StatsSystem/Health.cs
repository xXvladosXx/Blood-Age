using System;
using System.Collections.Generic;
using System.Linq;
using AttackSystem.AttackMechanics;
using Entity;
using InventorySystem;
using SaveSystem;
using StateMachine;
using UnityEngine;
using Random = System.Random;

namespace StatsSystem
{
    [Serializable]
    public class Health : ISavable, IRenewable
    {
        private float _currentHealth;
        private float _maxHealth;
        private bool _isDead;
        private FindStats _findStats;

        public event Action<AliveEntity> OnTakeHit;
        public event Action<float> OnHealthPctChanged = delegate(float f) { };
        public event Action OnDie;
        public event Action OnBloodyDie;
        public event Action OnStatRenewed;

        public float GetMaxHealth => _maxHealth;
        public float GetCurrentHealth => _currentHealth;

        public bool IsDead() => _isDead;

        public Health(FindStats findStats)
        {
            _currentHealth = findStats.GetStat(Characteristics.Health);
            _maxHealth = findStats.GetStat(Characteristics.Health);
            _findStats = findStats;
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

            var clearDamage = attackData.Damage;
            
            CalculateElementDamage(attackData);

            _currentHealth = Mathf.Clamp(_currentHealth - attackData.Damage, 0, _currentHealth);
            
            if(attackData.Vampiric)
                attackData.Damager.GetHealth.AddHealthPoints(attackData.Damage);

            float currentHealthPct = _currentHealth / _maxHealth;
            
            OnHealthPctChanged?.Invoke(currentHealthPct);
            OnTakeHit?.Invoke(attackData.Damager);
            
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
            if (num < attackData.CriticalChance)
            {
                attackData.Damage *= (attackData.CriticalDamage/100);
            }
        }

        private bool CheckEnemiesList(AttackData attackData)
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

        private void CalculateElementDamage(AttackData attackData)
        {
            var resistance = _findStats.GetComponent<ItemEquipper>();

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
            if (attackData.Accuracy > _findStats.GetStat(Characteristics.Evasion)) return false;
            
            Random random = new Random();
            var num = random.NextDouble() * 100;
            var limit = (attackData.Accuracy + _findStats.GetStat(Characteristics.Evasion))
                        * (100 - _findStats.GetStat(Characteristics.Evasion))/100;

            if (num > limit) return true;
            return false;
        }

        public object CaptureState()
        {
            var healthData = new HealthData
            {
                Health = _currentHealth,
                WasDead = _isDead
            };
            
            return healthData;
        }

        public void RestoreState(object state)
        {
            var healthData = (HealthData) state;

            _currentHealth = healthData.Health;
            _isDead = healthData.WasDead;
        }

        public void AddHealthPoints(float healthReg)
        {
            if(_currentHealth == _maxHealth) return;
            _currentHealth = Mathf.Clamp(_currentHealth + healthReg, 0, _maxHealth);

            float currentHealthPct = _currentHealth / _maxHealth;
            OnHealthPctChanged?.Invoke(currentHealthPct);
        }


        public void Renew()
        {
            _currentHealth = _findStats.GetStat(Characteristics.Health);
            _maxHealth = _findStats.GetStat(Characteristics.Health);
            _isDead = false;
            
            float currentHealthPct = _currentHealth / _maxHealth;
            
            OnHealthPctChanged?.Invoke(currentHealthPct);
            OnStatRenewed?.Invoke();
        }
        
        [Serializable]
        public class HealthData
        {
            public bool WasDead;
            public float Health;
        }
    }
}