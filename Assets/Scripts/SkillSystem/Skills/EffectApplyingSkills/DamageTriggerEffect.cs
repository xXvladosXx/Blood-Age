using System;
using System.Collections;
using System.Collections.Generic;
using AttackSystem.AttackMechanics;
using Entity;
using Runemark.Common;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using StatsSystem;
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


        public void AddData(Dictionary<string, float> data)
        {
            if(_criticalChance != 0)
                data.Add("Critical Chance", _criticalChance);
            
            if(_criticalDamage != 0)
                data.Add("Critical Damage", _criticalDamage);
            
            if(_damage != 0)
                data.Add("Damage", _damage);
           
            data.Add("Accuracy", 100);
        }
    }
}