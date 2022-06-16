using System;
using System.Collections.Generic;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem.Skills.TargetingSkills
{
    [CreateAssetMenu (menuName = "Skill/Targeting/Immediate")]
    public class ImmediatePointTargeting : Targeting
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _groundOffset = 1f;

        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out raycastHit, 1000, _layerMask))
            {
                skillData.MousePosition = raycastHit.point + ray.direction * (_groundOffset / ray.direction.y);

                if (Vector3.Distance(skillData.GetUser.transform.position, raycastHit.transform.position) < 1)
                {
                    finishedAttack();
                    return;
                }
                
                skillData.GetUser.transform.LookAt(raycastHit.point);
            }
            
            finishedAttack();
        }
    }
}