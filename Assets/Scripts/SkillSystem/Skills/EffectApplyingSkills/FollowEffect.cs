namespace SkillSystem.MainComponents.EffectApplyingSkills
{
    using System;
    using System.Collections;
    using DefaultNamespace.MouseSystem;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using DefaultNamespace.StateMachine;
    using DefaultNamespace.StateMachine.PlayerStates;
    using DefaultNamespace.UI.ButtonClickable;
    using InventorySystem;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [CreateAssetMenu (menuName = "Skill/Effect/Follow")]
    public class FollowEffect : EffectApplying
    {
        [SerializeField] private GameObject _particleEffect;
        [SerializeField] private float _delayToSpawn;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private GameObject _hitParticle;
        [SerializeField] private float _maxDamage;

        private Animator _animator;
        private static readonly int Canceled = Animator.StringToHash("Canceled");
        [SerializeField] private float _damage;

        public override void Effect(SkillData skillData, Action finished)
        {
            skillData.StartCoroutine(WaitUntilCanceled(skillData, finished));
            _animator = skillData.GetUser.GetComponent<Animator>();
        }

        private IEnumerator WaitUntilCanceled(SkillData skillData, Action finished)
        {
            GameObject particleEffect = null;

            yield return new WaitForSeconds(_delayToSpawn);
            
            particleEffect = Instantiate(_particleEffect, skillData.GetUser.transform);
            particleEffect.GetComponent<CollisionDetector>().ReceiveData(new AttackData
            {
                Damage = 0,
                MaxDamage = _maxDamage,
                Damager = skillData.GetUser.transform,
            }, _hitParticle);
            
            while (true)
            {
                RaycastHit raycastHit;
                skillData.GetUser.GetComponent<IStateSwitcher>().SwitchState<CastPlayerState>();

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out raycastHit,
                    1000, _layerMask))
                {
                    Vector3 lTargetDir = raycastHit.point - skillData.GetUser.transform.position;
                    lTargetDir.y = 0.0f;
                    
                    skillData.GetUser.transform.rotation = Quaternion.RotateTowards(skillData.GetUser.transform.rotation, 
                        Quaternion.LookRotation(lTargetDir), Time.time * _speed);
                }
               
                if (Mouse.current.rightButton.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Destroy(particleEffect);
                    _animator.SetBool(Canceled, true);
                    
                    var state = skillData.GetUser.GetComponent<IStateSwitcher>().GetCurrentState;

                    if (state is CastPlayerState castPlayerState)
                    {
                        castPlayerState.SwitchToIdle();
                    }
                    
                    finished();
                    yield break;
                }

                yield return null;
            }
        }
    }
}