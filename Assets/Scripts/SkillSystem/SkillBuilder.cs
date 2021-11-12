namespace SkillSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using SkillSystem.MainComponents;
    using UnityEngine;

    public class SkillBuilder : MonoBehaviour, IModifier
    {
        [SerializeField] private SkillNode[] _passiveSkill;

        /*
        public float AddBonus(Characteristics[] characteristics)
        {
            foreach (var skillNode in _passiveSkill)
            {
                if (skillNode is PassiveSkill passiveSkill)
                {
                    return passiveSkill.GiveBonus(characteristics);
                }
            }

            return 0;
        }
        */

        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IEnumerable<PlayerPassiveSkillBonus> AllMatchedPassiveSkillBonuses(PassiveSkill skill) =>
                skill._playerPassiveSkillBonus
                    .Where(x => characteristics.Contains(x.Characteristics));

            IBonus CharacteristicToBonus(Characteristics c, float value)
                => c switch {
                    Characteristics.Health => new HealthBonus(value),
                    Characteristics.Damage => new DamageBonus(value),
                    _ => throw new IndexOutOfRangeException()
                };

            return _passiveSkill
                .OfType<PassiveSkill>()
                .SelectMany(AllMatchedPassiveSkillBonuses)
                .Select(x => CharacteristicToBonus(x.Characteristics, x.Value));
        }
    }
}