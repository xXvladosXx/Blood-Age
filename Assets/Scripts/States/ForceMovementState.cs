using System.Collections.Generic;
using Entity;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace States
{
    [CreateAssetMenu (menuName = "State/ForceMovement")]
    public class ForceMovementState : StateData
    {
        [SerializeField] private AnimationCurve _speedCurve;
        [SerializeField] private float _speed;
        [SerializeField] private float _distance = 2;
        [SerializeField] private LayerMask _layerMask;

        private Rigidbody _rigidbody;
        private PlayerEntity _playerEntity;
        private Movement _movement;
        private Collider _collider;
        private List<GameObject> _raycasters;

        public override void OnEnter(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            _rigidbody = animator.GetComponent<Rigidbody>();
            _movement = animator.GetComponent<Movement>();
            _collider = animator.GetComponent<Collider>();
            _playerEntity = animator.GetComponent<PlayerEntity>();
            _raycasters = _playerEntity.GetRaycasters;
            
            _collider.isTrigger = false;
            _movement.EnableMovement(false);
        }

        public override void UpdateAbility(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            foreach (var raycaster in _raycasters)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(raycaster.transform.position, raycaster.transform.forward, out raycastHit,
                        _distance, 1 << LayerMask.NameToLayer("Unwalkable")))
                {
                    return;
                }
            }
            
            var transform = animator.transform;
            var position = transform.position;
            var forward = transform.forward;
            
            position += new Vector3(forward.x, forward.y, forward.z)
                        * _speed * _speedCurve.Evaluate(stateInfo.normalizedTime);
            
            animator.transform.position = position;
        }

        public override void OnExit(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            _rigidbody.velocity = Vector3.zero;
            _collider.isTrigger = true;
            _movement.EnableMovement(true);
        }
    }
}