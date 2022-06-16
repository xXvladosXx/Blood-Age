using System;
using System.Linq;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using UnityEngine;

namespace SkillSystem.Skills.TargetingSkills
{
    [CreateAssetMenu (menuName = "Skill/Targeting/ImmediateAI")]
    public class ImmediatePointTargetingAI : Targeting
    {
        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            skillData.GetUser.transform.LookAt(skillData.GetUser.Targets.FirstOrDefault()!.transform.position);
            
            finishedAttack();
        }
    }
}