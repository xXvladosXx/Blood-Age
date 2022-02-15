using Entity;
using StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace States
{
    [CreateAssetMenu (menuName = "State/MouseMovement")]
    public class MouseMovementState : StateData
    {
        [SerializeField] private AnimationCurve _speedCurve;
        [SerializeField] private float _speed;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _endTime;
        [SerializeField] private float _maxDistance;
        
        private Rigidbody _rigidbody;
        private Movement _movement;
        private Collider _collider;
        private float _positionY;
        private PlayerEntity _playerEntity;
        private RaycastHit _raycastHit;

        public override void OnEnter(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            _rigidbody = animator.GetComponent<Rigidbody>();
            _collider = animator.GetComponent<Collider>();
            _movement = animator.GetComponent<Movement>();
            _movement.EnableMovement(false);
            _playerEntity = animator.GetComponent<PlayerEntity>();
            _positionY = animator.transform.position.y;
            Physics.Raycast(_playerEntity.GetRay(), out _raycastHit, Mathf.Infinity, _layerMask);

            _collider.isTrigger = false;
        }

        public override void UpdateAbility(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime > _endTime)
            {
                _movement.Cancel();
                return;
            }
           
            _movement.StartMoveTo(_raycastHit.point, 1f,  _speed * _speedCurve.Evaluate(stateInfo.normalizedTime));
        }

        public override void OnExit(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            _rigidbody.velocity = Vector3.zero;
            _collider.isTrigger = true;
            _movement.EnableMovement(true);
        }
    }
}