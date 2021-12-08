namespace SkillSystem.MainComponents.EffectApplyingSkills
{
    using System;
    using System.Collections;
    using DefaultNamespace.Entity;
    using DefaultNamespace.MouseSystem;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using DefaultNamespace.UI.ButtonClickable;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Skill/Effect/DamageEffect")]
    public class DamageTriggerEffect : EffectApplying
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _criticalChance;
        [SerializeField] private float _criticalDamage;
        [SerializeField] private float _delay;
        [SerializeField] private GameObject _hitParticleEffect;
        public override void Effect(SkillData skillData, Action finished)
        {
            skillData.StartCoroutine(DelayDamage(skillData));
        }

        private IEnumerator DelayDamage(SkillData skillData)
        {
            yield return new WaitForSeconds(_delay);

            if (skillData.Target == null) yield break;
            
            foreach (var target in skillData.Target)
            {
                GameObject hitParticleEffect =
                    Instantiate(_hitParticleEffect, target.transform.position, Quaternion.identity);
                
                target.GetComponent<AliveEntity>().GetHealth.TakeHit(new AttackData
                {
                    Damage = _damage,
                    Damager = skillData.GetUser.transform,
                    CriticalChance = _criticalChance,
                    CriticalDamage = _criticalDamage
                });
                
                Destroy(hitParticleEffect, 1f);
            }
        }
    }
}