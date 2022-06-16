using System;
using System.Linq;
using DefaultNamespace;
using SaveSystem;
using StatsSystem.Bonuses;
using UnityEngine;

namespace StatsSystem
{
    public class FindStats : MonoBehaviour, ISavable
    {
        [SerializeField] private StarterCharacterData _starterCharacterData;
        [SerializeField] private GameObject _levelupParticle;
        
        private Class _class;
        private int _level = 1;
        
        public Class GetClass => _class;
        public int GetLevel => _level;

        public void SetClass(Class characterClass)
        {
            _class = characterClass;
        }
        public event Action OnLevelUp;
        public event Action OnLevelRestored;
        public void UpdateLevel(ref int currentLevel, float currentExp)
        {
            int newLevel = CalculateLevel(currentExp);

            if (newLevel <= currentLevel) return;
            
            currentLevel = newLevel;
            OnLevelUp?.Invoke();
            PlayLevelUpEffect();
        }

        private void PlayLevelUpEffect()
        {
            var levelEffect = Instantiate(_levelupParticle, transform);
            Destroy(levelEffect, 2f);
        }

        public int CalculateLevel(float currentExp)
        {
            int maxLevel = _starterCharacterData.GetLevels(_class, Characteristics.ExperienceToLevelUp);

            for (int level = 1; level < maxLevel; level++)
            {
                float expToLevelUp = _starterCharacterData.ReturnLevelValueCharacteristics(_class, Characteristics.ExperienceToLevelUp, level);

                if (expToLevelUp > currentExp)
                {
                    _level = level;

                    return level;
                }
            }

            _level = maxLevel + 1;
            
            return maxLevel + 1;
        }
        public float GetStat(Characteristics characteristics)
        {
            float starterValue = _starterCharacterData.ReturnLevelValueCharacteristics(_class, characteristics, _level);
            float valueWithBonus = GetBonus(characteristics) + starterValue;
            
            return valueWithBonus;
        }

        private float GetBonus(Characteristics characteristic)
        {
            bool IsBonusAssignableToCharacteristics(IBonus bonus) 
                => (bonus, characteristic) switch
                {
                    (HealthBonus b, Characteristics.Health) => true,
                    (DamageBonus b, Characteristics.Damage) => true,
                    (CriticalChanceBonus b, Characteristics.CriticalChance) => true,
                    (CriticalDamageBonus b, Characteristics.CriticalDamage) => true,
                    (DeathExperienceBonus b, Characteristics.DeathExperience) => true,
                    (AttackSpeedBonus b, Characteristics.AttackSpeed) => true,
                    (MovementSpeedBonus b, Characteristics.MovementSpeed) => true,
                    (ManaRegenerationBonus b, Characteristics.ManaRegeneration) => true,
                    (ManaBonus b, Characteristics.Mana) => true,
                    (HealthRegenerationBonus b, Characteristics.HealthRegeneration) => true,
                    (EvasionBonus b, Characteristics.Evasion) => true,
                    (AccuracyBonus b, Characteristics.Accuracy) => true,
                    _ => false
                };

            return GetComponents<IModifier>()
                .SelectMany(x => x.AddBonus(new[] { characteristic }))
                .Where(IsBonusAssignableToCharacteristics)
                .Sum(x => x.Value);
        }

        public object CaptureState()
        {
            return _level;
        }

        public void RestoreState(object state)
        {
            _level = (int) state;
            
            OnLevelRestored?.Invoke();
            Debug.Log(_level);
        }
    }
}