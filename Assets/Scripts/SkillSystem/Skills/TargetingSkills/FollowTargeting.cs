namespace SkillSystem.MainComponents.TargetingSkills
{
    using System;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using DefaultNamespace.UI.ButtonClickable;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [CreateAssetMenu (menuName = "Skill/Targeting/Follow")]
    public class FollowTargeting : Targeting, IManaChangeable
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _groundOffset = 1f;
        
        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            while (true)
            {

                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out raycastHit, 1000, _layerMask))
                    skillData.MousePosition = raycastHit.point + ray.direction * _groundOffset / ray.direction.y;

                finishedAttack();
            }
        }

        public void ChangeMana(float mana)
        {
            
        }
    }
}