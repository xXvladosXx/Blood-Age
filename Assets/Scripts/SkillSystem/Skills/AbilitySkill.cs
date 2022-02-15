using Entity;
using SkillSystem.SkillInfo;
using SkillSystem.SkillNodes;
using SkillSystem.Skills.PassiveAbilitySkill;
using StateMachine;
using StateMachine.BaseStates;
using States;
using UnityEngine;

namespace SkillSystem.Skills
{
    [CreateAssetMenu (menuName = "Skill/AbilitySkill")]
    public class AbilitySkill : ActiveSkill
    {
        [SerializeField] private StandardAbility[] _standardAbilitySkills;
        [SerializeField] private float _stamina;
        private IStateSwitcher _stateSwitcher;

        public override void ApplySkill(AliveEntity user)
        {
            if (!user.GetStamina.HasEnoughStamina(_stamina))
            {
                Cancel();
                return;
            }
            
            base.ApplySkill(user);
            if(_skillData.IsCancelled) return;
            foreach (var standardAbilitySkill in _standardAbilitySkills)
            {
                standardAbilitySkill.ApplyAbility(_skillData);
            }
        }
    }

}