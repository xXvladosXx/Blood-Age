namespace SkillSystem.MainComponents.TargetingSkills
{
    using System;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using DefaultNamespace.UI.ButtonClickable;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [CreateAssetMenu (menuName = "Skill/Targeting/Immediate")]
    public class ImmediateTargeting : Targeting
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _groundOffset = 1f;

        private StarterAssetsInputs _player;
        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            _player = skillData.GetUser.GetComponent<StarterAssetsInputs>();

            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out raycastHit, 1000, _layerMask))
            {
                skillData.MousePosition = raycastHit.point + ray.direction * _groundOffset / ray.direction.y;
                skillData.GetUser.transform.LookAt(raycastHit.point);
            }

            finishedAttack();
        }
    }
}