using System;
using System.Collections;
using System.Collections.Generic;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem.Skills.TargetingSkills
{
    [CreateAssetMenu(fileName = "Targeting", menuName = "Skill/Targeting/ContinuousApplying", order = 0)]
    public class ImmediateSelfApplying : Targeting, ICollectable
    {
        [SerializeField] private float _skillRadius;

        private StarterAssetsInputs _user;
        private Animator _animator;
        private GameObject _skillRenderer;

        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            _user = skillData.GetUser.GetComponent<StarterAssetsInputs>();
            _animator = _user.GetComponent<Animator>();
            var position = skillData.GetUser.transform.position;
            skillData.MousePosition = new Vector3(position.x, position.y+0.1f, position.z);
            skillData.Targets = GetGameobjectsInRadius(position);

            finishedAttack();
        }

        private IEnumerable<GameObject> GetGameobjectsInRadius(Vector3 point)
        {
            var hits = Physics.SphereCastAll(point, _skillRadius, Vector3.up, 100);

            foreach (var raycastHit in hits)
            {
                yield return raycastHit.collider.gameObject;
            }
        }

        public void AddData(Dictionary<string, float> data)
        {
            if(_skillRadius != 0)
                data.Add("Radius", _skillRadius);
        }
    }
}