namespace DefaultNamespace.PlayerStates
{
    using DefaultNamespace.Entity;
    using UnityEngine;
    using UnityEngine.AI;

    [CreateAssetMenu (fileName = ("Movement"), menuName = "State/PlayerMovement")]

    public class PlayerMovementState : IdleMovementState
    {
        private NavMeshAgent _navMeshAgent;
        private StarterAssetsInputs _starterAssetsInputs;
        private AliveEntity _aliveEntity;
        
        private static readonly int Roll = Animator.StringToHash("Roll");

        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {           
            base.OnEnter(characterStateBase, animator, stateInfo);
            _navMeshAgent = animator.GetComponent<NavMeshAgent>();
            _starterAssetsInputs = animator.GetComponent<StarterAssetsInputs>();
            _aliveEntity = animator.GetComponent<AliveEntity>();
            _navMeshAgent.enabled = true;
            
            Movement.Cancel();
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            // if (_starterAssetsInputs.RollInput)
            // {
            //     animator.SetBool(Roll, true);
            // }
            if(_starterAssetsInputs.enabled == false) return;
            if(AliveEntity.GetHealth.IsDead()) return;
            
            PlayerInput(animator);
            
            if(AliveEntity.GetItemEquipper == null) return;
            DistanceToAttack = AliveEntity.GetItemEquipper.GetAttackRange;
        }

        private void PlayerInput(Animator animator)
        {
            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(_starterAssetsInputs.GetRay(), out raycastHit);
            if (!hasHit) return;

            if (_starterAssetsInputs.ButtonInput)
            {
                animator.SetBool(ForceTransition, false);
                if (raycastHit.collider.GetComponent<AliveEntity>() != null && 
                    (raycastHit.collider.GetComponent<AliveEntity>() != _aliveEntity))
                {
                    AttackRegistrator.AttackData.Target = raycastHit.transform;
                }

                Movement.StartMoveTo(raycastHit.point, 1f);
            }
            
            if (AttackRegistrator.AttackData.Target != null)
            {
                var distanceToTargetSec =
                    Vector3.Distance(animator.transform.position, AttackRegistrator.AttackData.Target.position);

                if (distanceToTargetSec < DistanceToAttack)
                {
                    Movement.Cancel();
                    animator.transform.LookAt(AttackRegistrator.AttackData.Target.position);
                    animator.SetBool(Attack, true);
                }
                else
                {
                    Movement.StartMoveTo(AttackRegistrator.AttackData.Target.position, 1f);
                }
            }
        }
    }
}