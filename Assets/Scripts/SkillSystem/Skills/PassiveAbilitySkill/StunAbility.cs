using AttackSystem.AttackMechanics;
using SkillSystem.SkillInfo;
using StateMachine;
using StateMachine.BaseStates;
using UnityEngine;

namespace SkillSystem.Skills.PassiveAbilitySkill
{
    [CreateAssetMenu (menuName = "Ability/StunAbility")]
    public class StunAbility : StandardAbility
    {
        [SerializeField] private float _stunDuration;
        
        public override void ApplyAbility(SkillData skillData)
        {
            skillData.Target.GetHealth.TakeHit(
                new AttackData
                {
                    Damager = skillData.GetUser,
                    Damage = Damage,
                    CriticalChance = CriticalChance,
                    CriticalDamage = CriticalDamage
                });

            var switcher = skillData.Target.GetComponent<IStateSwitcher>();
            var state = switcher.SwitchState<StunBaseState>();
            //state?.StartState(_stunDuration);
        }
    }
}