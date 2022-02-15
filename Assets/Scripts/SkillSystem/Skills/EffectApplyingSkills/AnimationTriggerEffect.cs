using System;
using System.Collections;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using UnityEngine;

namespace SkillSystem.Skills.EffectApplyingSkills
{
    [CreateAssetMenu (menuName = "Skill/Effect/AnimationEffect")]
    public class AnimationTriggerEffect : ContinuousEffectApplying
    {
        [SerializeField] private string _animationSkill = "";
        [SerializeField] private float _delay;

        private Animator _userAnimator;
        
        public override void Effect(SkillData skillData, Action cancel, Action finished)
        {
            _userAnimator = skillData.GetUser.GetComponent<Animator>();
            
            _userAnimator.Play(_animationSkill);
            
            skillData.StartCoroutine(WaitToPlayAnimation(finished, skillData));
        }
        private IEnumerator WaitToPlayAnimation(Action finished, SkillData skillData)
        {
            yield return new WaitForSeconds(_delay);

            finished();
        }
    }
}