namespace SkillSystem.MainComponents.EffectApplyingSkills
{
    using System;
    using System.Collections;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using DefaultNamespace.StateMachine;
    using DefaultNamespace.StateMachine.PlayerStates;
    using DefaultNamespace.UI.ButtonClickable;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Skill/Effect/AnimationEffect")]
    public class AnimationTriggerEffect : EffectApplying
    {
        [SerializeField] private string _animationSkill = "";
        [SerializeField] private float _delay;

        private Animator _userAnimator;
        
        public override void Effect(SkillData skillData, Action finished)
        {
            _userAnimator = skillData.GetUser.GetComponent<Animator>();
            
            _userAnimator.Play(_animationSkill);
            
            skillData.StartCoroutine(WaitToPlayAnimation(finished, skillData));
        }

        private IEnumerator WaitToPlayAnimation(Action finished, SkillData skillData)
        {
            yield return new WaitForSeconds(_delay);
            
            if (_delay != 0)
            {
                var state = skillData.GetUser.GetComponent<IStateSwitcher>().GetCurrentState;

                if (state is CastPlayerState castPlayerState)
                {
                    castPlayerState.SwitchToIdle();
                }
            }

            finished();
        }
    }
}