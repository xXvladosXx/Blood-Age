using System;
using System.Collections;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using StatsSystem;
using UnityEngine;

namespace SkillSystem.Skills.EffectApplyingSkills
{
    [CreateAssetMenu (menuName = "Skill/Effect/BonusToCharacteristic")]
    public class BonusEffect : EffectApplying
    {
        [SerializeField] private string _bonus;
        [SerializeField] private float _modifier;
        [SerializeField] private float _time;
        [SerializeField] public Buff[] _temporaryBuffs;
        
        private float _startValue;
        private Animator _animator;
        
        public override void Effect(SkillData skillData, Action cancel, Action finished)
        {
            _animator = skillData.GetUser.GetComponent<Animator>();
            _startValue = _animator.GetFloat(_bonus);
            _animator.SetFloat(_bonus, _modifier);
            skillData.StartCoroutine(WaitToDisableBonus(skillData));
        }
        private IEnumerator WaitToDisableBonus(SkillData skillData)
        {
            float time = 0;

            skillData.GetUser.GetComponent<BuffApplier>().SetBuff(_temporaryBuffs);

            while (true)
            {
                time += Time.deltaTime;

                if (time > _time)
                {
                    _animator.SetFloat(_bonus, _startValue);
                    yield break;
                }
                
                yield return null;
            }
        }
    }
}