using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using UnityEngine;

namespace SkillSystem.Skills.EffectApplyingSkills
{
    [CreateAssetMenu(menuName = "Skill/Effect/HealEffect")]
    public class HealthTriggerEffect : EffectApplying, ICollectable
    {
        [SerializeField] private float _healingValue;
        [UnityEngine.Min(1)]
        [SerializeField] private int _waves;
        [SerializeField] private float _delay;
        public override void Effect(SkillData skillData, Action cancel, Action finished)
        {
            skillData.GetUser.StartCoroutine(WaitToHeal(skillData, finished, cancel));
        }

        private IEnumerator WaitToHeal(SkillData skillData, Action finished, Action cancel)
        {
            for (int i = 0; i < _waves; i++)
            {
                foreach (var target in skillData.Targets)
                {
                    target.TryGetComponent(out IHealable healable);
                    healable.Heal(_healingValue);
                }
                yield return new WaitForSeconds(_delay);
            }
            

            finished();
            cancel();
        }

        public void AddData(Dictionary<string, StringBuilder> data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Healing: ").Append(_healingValue).AppendLine();
            stringBuilder.Append("Delay: ").Append(_delay).AppendLine();
            stringBuilder.Append("Waves: ").Append(_waves).AppendLine();
            
            data.Add("Healing effects: ", stringBuilder);
        }
    }
}