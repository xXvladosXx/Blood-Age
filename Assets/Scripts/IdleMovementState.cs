namespace DefaultNamespace
{
    using System;
    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.InputSystem;

    [CreateAssetMenu (fileName = ("Movement"), menuName = "State/Movement")]
    public class IdleMovementState : StateData
    {
        [SerializeField] private bool _manualMovement = false;

        private Movement _movement;
        private StarterAssetsInputs _starterAssetsInputs;
        private NavMeshAgent _navMeshAgent;

        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            _movement = animator.GetComponent<Movement>();
            _starterAssetsInputs = animator.GetComponent<StarterAssetsInputs>();
            _navMeshAgent = animator.GetComponent<NavMeshAgent>();
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(_starterAssetsInputs.GetRay(), out raycastHit);

            if (!hasHit) return;
            if (_starterAssetsInputs.ButtonInput)
            {
                _movement.StartMoveTo(raycastHit.point, 1f);
            }
        }

        
        public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }
}