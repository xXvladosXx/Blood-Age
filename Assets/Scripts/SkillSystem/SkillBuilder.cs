namespace SkillSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using SkillSystem.MainComponents;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class SkillBuilder : MonoBehaviour, IModifier
    {
        [SerializeField] private SkillNode[] _skills;
        public SkillNode[] GetSkillNodes => _skills;

        private StarterAssetsInputs _starterAssetsInputs;

        private void Awake()
        {
            _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        }
        
        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IEnumerable<CharacteristicBonus> AllMatchedPassiveSkillBonuses(PassiveSkill skill) =>
                skill._playerPassiveSkillBonus
                    .Where(x => characteristics.Contains(x.Characteristics));

            IBonus CharacteristicToBonus(Characteristics c, float value)
                => c switch {
                    Characteristics.CriticalChance => new CriticalChanceBonus(value),
                    Characteristics.CriticalDamage => new CriticalDamageBonus(value),
                    Characteristics.Health => new HealthBonus(value),
                    Characteristics.Damage => new DamageBonus(value),
                    _ => throw new IndexOutOfRangeException()
                };

            return _skills
                .OfType<PassiveSkill>()
                .SelectMany(AllMatchedPassiveSkillBonuses)
                .Select(x => CharacteristicToBonus(x.Characteristics, x.Value));
        }
    }
}