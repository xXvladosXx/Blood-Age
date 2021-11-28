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
        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            _rigidbody = animator.GetComponent<Rigidbody>();
            _movement = animator.GetComponent<NavMeshAgent>();
            _collider = animator.GetComponent<Collider>();
            
            _movement.enabled = false;
            _collider.isTrigger = false;
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            var position = animator.transform.position;
            
            position += new Vector3(animator.transform.forward.x,
                0, animator.transform.forward.z) * _speed * _speedCurve.Evaluate(stateInfo.normalizedTime);
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