namespace SkillSystem.MainComponents.EffectApplyingSkills
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using DefaultNamespace.UI.ButtonClickable;
    using Sirenix.OdinInspector;
    using StatsSystem;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Skill/Effect/BonusToCharacteristic")]
    public class BonusEffect : EffectApplying
    {
        [SerializeField] private string _bonus;
        [SerializeField] private float _modifier;
        [SerializeField] private float _time;
        [SerializeField] public Buff[] _temporaryBuffs;
        
        private float _startValue;

        public event Action OnBonusApplied;
        
        public override void Effect(SkillData skillData, Action finished)
        {
            _startValue = skillData.GetUser.GetComponent<Animator>().GetFloat(_bonus);
            skillData.GetUser.GetComponent<Animator>().SetFloat(_bonus, _modifier);
            skillData.StartCoroutine(WaitToDisableBonus(skillData));
        }

        private IEnumerator WaitToDisableBonus(SkillData skillData)
        {
            float time = 0;
          
            OnBonusApplied?.Invoke();
            skillData.GetUser.GetComponent<BuffApplier>().SetBuff(_temporaryBuffs);
            
            while (true)
            {
                time += Time.deltaTime;

                if (time > _time)
                {
                    skillData.GetUser.GetComponent<Animator>().SetFloat(_bonus, _startValue);
                    
                    yield break;
                }
                
                yield return null;
            }
        }
    }
}