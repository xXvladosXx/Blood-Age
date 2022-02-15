using AttackSystem.AttackMechanics;
using Entity;
using SkillSystem.SkillInfo;
using StateMachine;
using StateMachine.BaseStates;
using UnityEngine;

namespace SkillSystem.Skills.PassiveAbilitySkill
{
    [CreateAssetMenu (menuName = "Ability/PushAbility")]
    public class PushAbility : StandardAbility
    {
        [SerializeField] private float _force;
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

                    Debug.Log("Pushed");
                    aliveEntity.GetComponent<Rigidbody>().AddForce(-aliveEntity.transform.forward * _force, ForceMode.Impulse);
                }
            }
        }
    }
}