namespace DefaultNamespace.PlayerStates
{
    using UnityEngine;
    using UnityEngine.AI;

    [CreateAssetMenu (fileName = ("Movement"), menuName = "State/PlayerMovement")]

    public class PlayerMovementState : IdleMovementState
    {
        private NavMeshAgent _navMeshAgent;
        private StarterAssetsInputs _starterAssetsInputs;
        
        private static readonly int Roll = Animator.StringToHash("Roll");

        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {           
            base.OnEnter(characterStateBase, animator, stateInfo);
            _navMeshAgent = animator.GetComponent<NavMeshAgent>();
            _starterAssetsInputs = animator.GetComponent<StarterAssetsInputs>();
            _navMeshAgent.enabled = true;
            
            _movement.Cancel();
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            // if (_starterAssetsInputs.RollInput)
            // {
            //     animator.SetBool(Roll, true);
            // }
            
            PlayerInput(animator);
            
            if(_itemEquipper == null) return;
            _distanceToAttack = _itemEquipper.GetAttackRange;
        }

        private void PlayerInput(Animator animator)
        {
            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(_starterAssetsInputs.GetRay(), out raycastHit);

            if (_starterAssetsInputs.ButtonInput)
            {
                Debug.Log("SWWD");
            }

            if (!hasHit) return;

            if (_starterAssetsInputs.ButtonInput)
            {
                animator.SetBool(ForceTransition, false);
                if (raycastHit.collider.GetComponent<Health>() != null && (raycastHit.collider.GetComponent<Health>() != animator.GetComponent<Health>()))
                {
                    _attackRegistrator.AttackData.Target = raycastHit.transform;
                }

                _movement.StartMoveTo(raycastHit.point, 1f);
            }
            
            if (_attackRegistrator.AttackData.Target != null)
            {
                var distanceToTargetSec =
                    Vector3.Distance(animator.transform.position, _attackRegistrator.AttackData.Target.position);

                if (distanceToTargetSec < _distanceToAttack)
                {
                    _movement.Cancel();
                    animator.transform.LookAt(_attackRegistrator.AttackData.Target.position);
                    animator.SetBool(Attack, true);
                }
                else
                {
                    _movement.StartMoveTo(_attackRegistrator.AttackData.Target.position, 1f);
                }
            }
        }
    }
}