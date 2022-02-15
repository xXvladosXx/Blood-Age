using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private float _criticalChance;
        [SerializeField] private float _criticalDamage;
        [SerializeField] private float _damage;
        [SerializeField] private float _manaPerSecond;
        [SerializeField] private float _staminaPerSecond;
        
        private Camera _camera;

        public override void Effect(SkillData skillData, Action cancel, Action finished)
        {
            skillData.StartCoroutine(WaitUntilCanceled(skillData, cancel, finished));
            _camera = Camera.main;
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

        public void AddData(Dictionary<string, float> data)
        {
            if(_criticalChance != 0)
                data.Add("Critical Chance", _criticalChance);
            
            if(_criticalDamage != 0)
                data.Add("Critical Damage", _criticalDamage);
            
            if(_damage != 0)
                data.Add("Damage", _maxDamage);
            data.Add("Accuracy", 100);
        }
    }
}