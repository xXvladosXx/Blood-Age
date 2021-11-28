namespace SkillSystem.MainComponents.EffectApplyingSkills
{
    using System;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using DefaultNamespace.UI.ButtonClickable;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Skill/Effect/AnimationEffect")]
    public class AnimationTriggerEffect : EffectApplying
    {
        [SerializeField] private string _animationSkill = "";

        private Animator _userAnimator;
        
        public override void Effect(SkillData skillData, Action finished)
        {
            _userAnimator = skillData.GetUser.GetComponent<Animator>();
            
            _userAnimator.Play(_animationSkill);
            finished();
        }
    }
}