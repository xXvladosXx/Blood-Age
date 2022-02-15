using SkillSystem.SkillInfo;
using UnityEngine;

namespace SkillSystem.Skills.PassiveAbilitySkill
{
    public abstract class StandardAbility : ScriptableObject
    {
        [SerializeField] protected float Damage;
        [SerializeField] protected float CriticalChance;
        [SerializeField] protected float CriticalDamage;
        [SerializeField] protected float Delay;
        public abstract void ApplyAbility(SkillData skillData);
    }
}