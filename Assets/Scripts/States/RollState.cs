namespace DefaultNamespace
{
    using UnityEngine;
    using UnityEngine.AI;

    [CreateAssetMenu (menuName = "State/Roll")]
    public class RollState : StateData
    {
        [SerializeField] private AnimationCurve _speedCurve;
        [SerializeField] private float _speed;

        private Rigidbody _rigidbody;
        private NavMeshAgent _movement;
        private Collider _collider;
        private float _positionY;
        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            _rigidbody = animator.GetComponent<Rigidbody>();
            _movement = animator.GetComponent<NavMeshAgent>();
            _collider = animator.GetComponent<Collider>();
            _positionY = animator.transform.position.y;
            
            _movement.enabled = false;
            _collider.isTrigger = false;
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            var position = animator.transform.position;
            
            position += new Vector3(animator.transform.forward.x,
                _positionY, animator.transform.forward.z) * _speed * _speedCurve.Evaluate(stateInfo.normalizedTime);
            position.y = _positionY;
            animator.transform.position = position;
        }

        public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            _rigidbody.velocity = Vector3.zero;
            _collider.isTrigger = true;
            _movement.enabled = true;
        }
    }
}