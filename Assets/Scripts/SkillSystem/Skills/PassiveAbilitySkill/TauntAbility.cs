using AttackSystem.AttackMechanics;
using Entity;
using SkillSystem.SkillInfo;
using StateMachine;
using StateMachine.BaseStates;
using UnityEngine;

namespace SkillSystem.Skills.PassiveAbilitySkill
{
    [CreateAssetMenu (menuName = "Ability/TauntAbility")]
    public class TauntAbility: StandardAbility
    {
        [SerializeField] private float _tauntDuration;

        public override void ApplyAbility(SkillData skillData)
        {
            foreach (var target in skillData.Targets)
            {
                target.TryGetComponent(out AliveEntity aliveEntity);
                if(aliveEntity != null)
                {
                    aliveEntity.GetHealth.TakeHit(
                        new AttackData
                        {
                            Damager = skillData.GetUser,
                            Damage = Damage,
                            CriticalChance = CriticalChance,
                            CriticalDamage = CriticalDamage
                        });

                    //aliveEntity.GetComponent<IStateSwitcher>().SwitchState<TauntBaseState>(_tauntDuration);
                }
            }
        }
    }
}