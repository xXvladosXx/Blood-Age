namespace DefaultNamespace
{
    using System;
    using System.Linq;
    using DefaultNamespace.Entity;
    using StatsSystem;
    using UnityEngine;

    public class FindStats : MonoBehaviour
    {
        [SerializeField] private StarterCharacterData _starterCharacterData;
        private Class _class;
        
        public void SetClass(Class characterClass)
        {
            _class = characterClass;
        }
        public event Action OnLevelUp;
        public void UpdateLevel(ref int currentLevel, LevelUp levelUp)
        {
            int newLevel = CalculateLevel(levelUp);

            if (newLevel <= currentLevel) return;
            
            currentLevel = newLevel;
            OnLevelUp?.Invoke();
        }

        private int CalculateLevel(LevelUp levelUp)
        {
            int maxLevel = _starterCharacterData.GetLevels(_class, Characteristics.ExperienceToLevelUp);

            for (int level = 1; level < maxLevel; level++)
            {
                float expToLevelUp = _starterCharacterData.ReturnLevelValueCharacteristics(_class, Characteristics.ExperienceToLevelUp, level);

                if (expToLevelUp > levelUp.GetCurrentExp)
                {
                    return level;
                }
            }

            return maxLevel + 1;
        }
        public float GetStat(Characteristics characteristics)
        {
            float starterValue = _starterCharacterData.ReturnLevelValueCharacteristics(_class, characteristics, 1);
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
                    _ => false
                };

            return GetComponents<IModifier>()
                .SelectMany(x => x.AddBonus(new[] { characteristic }))
                .Where(IsBonusAssignableToCharacteristics)
                .Sum(x => x.Value);
        }
    }
}