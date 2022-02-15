using System;
using System.Collections.Generic;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem.Skills.TargetingSkills
{
    [CreateAssetMenu (menuName = "Skill/Targeting/RadiusImmediate")]
    public class ImmediateRadiusTargeting : Targeting, ICollectable
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _groundOffset = 1f;
        [SerializeField] private float _distance = 5;
        [SerializeField] private float _radius;
        [SerializeField] private bool _rotationChange;
        [SerializeField] private GameObject _radiusDisplay;

        private StarterAssetsInputs _player;
        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            _player = skillData.GetUser.GetComponent<StarterAssetsInputs>();
            var playerRotation = _player.transform.rotation;
                        
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out raycastHit, 1000, _layerMask))
            {
                Debug.Log(raycastHit.collider.gameObject);
                var transform = skillData.GetUser.transform;
                if (_rotationChange)
                {
                    transform.LookAt(raycastHit.point);
                }

                Vector3 playerPos = transform.position;
                Vector3 playerDirection = transform.forward;
                
                Vector3 spawnPos = playerPos + playerDirection*_distance;
                
                skillData.Targets = GetGameobjectsInRadius(spawnPos);
                skillData.GetUser.transform.rotation = new Quaternion(playerRotation.x, transform.rotation.y, playerRotation.z, transform.rotation.w);
                skillData.MousePosition = spawnPos;
            }
            
            finishedAttack();
        }

        public void AddData(Dictionary<string, float> data)
        {
            if(_distance != 0)
                data.Add("Distance", _distance);
            if(_radius != 0)
                data.Add("Radius", _radius);
        }
        
        private IEnumerable<GameObject> GetGameobjectsInRadius(Vector3 point)
        {
            var hits = Physics.SphereCastAll(point, _radius, Vector3.up, 100);

            foreach (var raycastHit in hits)
            {
                yield return raycastHit.collider.gameObject;
            }
        }
    }
}