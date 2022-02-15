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

        private Rigidbody _rigidbody;
        private Movement _movement;
        private Collider _collider;
        private float _positionY;
        public override void OnEnter(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            _rigidbody = animator.GetComponent<Rigidbody>();
            _movement = animator.GetComponent<Movement>();
            _collider = animator.GetComponent<Collider>();
            _positionY = animator.transform.position.y;
            
            _collider.isTrigger = false;
            _movement.EnableMovement(false);
        }

        public override void UpdateAbility(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
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