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
        [SerializeField] private float _distanceToAttack = 15f;

        private Movement _movement;
        private StarterAssetsInputs _starterAssetsInputs;
        private AttackMaker _attackMaker;
        
        private Transform _target;
        
        private static readonly int ForceTransition = Animator.StringToHash("ForceTransition");
        private static readonly int Attack = Animator.StringToHash("Attack");

        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            _attackMaker = animator.GetComponentInChildren<AttackMaker>();
            _movement = animator.GetComponent<Movement>();
            _starterAssetsInputs = animator.GetComponent<StarterAssetsInputs>();
            
            animator.SetBool(ForceTransition, false);
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (_attackMaker.AttackData.Target != null)
            {
                var distanceToTargetSec =
                    Vector3.Distance(animator.transform.position, _attackMaker.AttackData.Target.position);
                
                if (distanceToTargetSec < _distanceToAttack)
                {
                    _movement.Cancel();
                    animator.transform.LookAt(_attackMaker.AttackData.Target.position);
                    animator.SetBool(Attack, true);
                    return;
                }
            }

            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(_starterAssetsInputs.GetRay(), out raycastHit);
            
            if (!hasHit) return;
            
            if (_starterAssetsInputs.ButtonInput)
            {
                animator.SetBool(ForceTransition, false);
                if (raycastHit.collider.GetComponent<Enemy>() != null)
                {
                    _attackMaker.AttackData.Target = raycastHit.transform;
                }
                
                _movement.StartMoveTo(raycastHit.point, 1f);
            }
        }

        
        public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(ForceTransition, false);
        }
    }
}