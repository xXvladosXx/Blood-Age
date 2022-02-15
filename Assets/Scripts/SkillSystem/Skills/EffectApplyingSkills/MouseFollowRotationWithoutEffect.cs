using System;
using System.Collections;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using SkillSystem.Skills.TargetingSkills;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem.Skills.EffectApplyingSkills
{
    [CreateAssetMenu (menuName = "Skill/Effect/FollowWithoutEffect")]

    public class MouseFollowRotationWithoutEffect : ContinuousEffectApplying
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _duration;

        private Camera _camera;

        public override void Effect(SkillData skillData, Action cancel, Action finished)
        {
            _camera = Camera.main;
            skillData.GetUser.StartCoroutine(RotateEntity(skillData, cancel, finished));
        }

        private IEnumerator RotateEntity(SkillData skillData, Action cancel, Action finished)
        {
            float time = 0;
            
            while (true)
            {
                time += Time.deltaTime;
                RaycastHit raycastHit;

                if (Physics.Raycast(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out raycastHit,
                        1000, _layerMask))
                {
                    Vector3 lTargetDir = raycastHit.point - skillData.GetUser.transform.position;
                    lTargetDir.y = 0.0f;
                    
                    skillData.GetUser.transform.rotation = Quaternion.RotateTowards(skillData.GetUser.transform.rotation, 
                        Quaternion.LookRotation(lTargetDir), Time.time * _speed);
                }
               
                if (time > _duration)
                {
                    cancel();
                    finished();
                    yield break;
                }

                yield return null;
            }
        }
    }
}