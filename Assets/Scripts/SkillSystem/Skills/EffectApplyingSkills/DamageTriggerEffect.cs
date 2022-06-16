using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AttackSystem.AttackMechanics;
using Entity;
using Runemark.Common;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using StatsSystem;
using UI.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace SkillSystem.Skills.EffectApplyingSkills
{
    [CreateAssetMenu(menuName = "Skill/Effect/DamageEffect")]
    public class DamageTriggerEffect : EffectApplying, ICollectable
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _criticalChance;
        [SerializeField] private float _criticalDamage;
        [SerializeField] private float _delay;
        [SerializeField] private GameObject _hitParticleEffect;
        [SerializeField] private bool _vampiric;
        [UnityEngine.Min(1)]
        [SerializeField] private int _waves;
        
        public override void Effect(SkillData skillData, Action cancel, Action finished)
        {
            for (int i = 0; i < _waves; i++)
            {
                skillData.StartCoroutine(DelayDamage(skillData));
            }
        }

        private IEnumerator DelayDamage(SkillData skillData)
        {
            yield return new WaitForSeconds(_delay);

            if (skillData.Targets == null) yield break;

            foreach (var target in skillData.Targets)
            {
                if (_hitParticleEffect != null)
                {
                    GameObject hitParticleEffect =
                        Instantiate(_hitParticleEffect, target.transform.position, Quaternion.identity);
                    
                    Destroy(hitParticleEffect, 1f);
                }
                target.TryGetComponent(out AliveEntity aliveEntity);
                if(aliveEntity != null)
                {
                    HealthBarEntity.Instance.ShowHealth(aliveEntity);
                    
                    aliveEntity.GetHealth.TakeHit(new AttackData
                    {
                        Damage = _damage + skillData.GetUser.GetStat(Characteristics.Damage),
                        Damager = skillData.GetUser,
                        CriticalChance = _criticalChance,
                        CriticalDamage = _criticalDamage,
                        Accuracy = 100,
                    });
                }
            }

        }


        public void AddData(Dictionary<string, StringBuilder> data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Damage: ").Append(_damage).AppendLine();
            stringBuilder.Append("Critical chance: ").Append(_criticalChance).AppendLine();
            stringBuilder.Append("Critical damage: ").Append(_criticalDamage).AppendLine();
            stringBuilder.Append("Accuracy: ").Append(100).AppendLine();
            
            data.Add("Damage effects: ", stringBuilder);
        }
    }
}