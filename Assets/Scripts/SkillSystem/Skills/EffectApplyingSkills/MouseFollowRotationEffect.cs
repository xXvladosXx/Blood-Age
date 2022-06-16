using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AttackSystem.AttackMechanics;
using DefaultNamespace.SkillSystem.SkillInfo;
using Entity;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using StateMachine;
using StateMachine.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem.Skills.EffectApplyingSkills
{
    [CreateAssetMenu(menuName = "Skill/Effect/Follow")]
    public class MouseFollowRotationEffect : ContinuousEffectApplying, ICollectable
    {
        [SerializeField] private GameObject _particleEffect;
        [SerializeField] private float _delayToSpawn;
        [SerializeField] private GameObject _hitParticle;
        [SerializeField] private float _maxDamage;
        [SerializeField] private float _delayToDamage;
        [SerializeField] private float _damage;
        [SerializeField] private float _manaPerSecond;
        [SerializeField] private float _staminaPerSecond;
        [SerializeField] private float _timeToDestroy;
        
        public override void Effect(SkillData skillData, Action cancel, Action finished)
        {
            skillData.StartCoroutine(WaitUntilCanceled(skillData, cancel, finished));
        }

        private IEnumerator WaitUntilCanceled(SkillData skillData, Action cancel, Action finished)
        {
            GameObject particleEffect = null;
            yield return new WaitForSeconds(_delayToSpawn);

            if (_particleEffect != null)
            {
                particleEffect = Instantiate(_particleEffect, skillData.GetUser.transform);
                particleEffect.GetComponent<CollisionDetector>().SendData(new AttackData
                {
                    Damage = _maxDamage,
                    MaxDamage = _maxDamage,
                    Damager = skillData.GetUser,
                    Accuracy = 100,
                }, _hitParticle, _delayToDamage);
            }
            
            if(_timeToDestroy != 0)
                Destroy(particleEffect, _timeToDestroy);

            while (true)
            {
                if (!skillData.GetUser.GetMana.HasEnoughMana(_manaPerSecond))
                {
                    ExecuteState(cancel, finished, particleEffect);
                    yield break;
                }
                
                if (!skillData.GetUser.GetStamina.HasEnoughStamina(_staminaPerSecond))
                {
                    ExecuteState(cancel, finished, particleEffect);
                    yield break;
                }
                
                if (Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
                {
                    ExecuteState(cancel, finished, particleEffect);
                    yield break;
                }

                yield return null;
            }
        }

        private static void ExecuteState(Action cancel, Action finished, GameObject particleEffect)
        {
            Destroy(particleEffect);

            cancel();
            finished();
        }

        public void AddData(Dictionary<string, StringBuilder> data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Start damage: ").Append(_damage).AppendLine();
            stringBuilder.Append("Max damage: ").Append(_maxDamage).AppendLine();
            stringBuilder.Append("Delay: ").Append(_delayToSpawn).AppendLine();
            stringBuilder.Append("Stamina per second: ").Append(_staminaPerSecond).AppendLine();
            stringBuilder.Append("Mana per second: ").Append(_manaPerSecond).AppendLine();
            
            data.Add("Continuous damage effects: ", stringBuilder);
        }
    }
}