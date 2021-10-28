namespace DefaultNamespace.PlayerStates
{
    using UnityEngine;

    [CreateAssetMenu (fileName = ("Movement"), menuName = "State/PlayerMovement")]

    public class PlayerMovementState : IdleMovementState
    {
        private StarterAssetsInputs _starterAssetsInputs;

        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {           
            base.OnEnter(characterStateBase, animator, stateInfo);
            _starterAssetsInputs = animator.GetComponent<StarterAssetsInputs>();
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            PlayerInput(animator);
        }

        private void PlayerInput(Animator animator)
        {
            if (_attackRegistrator.AttackData.Target != null)
            {
                var distanceToTargetSec =
                    Vector3.Distance(animator.transform.position, _attackRegistrator.AttackData.Target.position);

                if (distanceToTargetSec < _distanceToAttack)
                {
                    _movement.Cancel();
                    animator.transform.LookAt(_attackRegistrator.AttackData.Target.position);
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
                    _attackRegistrator.AttackData.Target = raycastHit.transform;
                }

                _movement.StartMoveTo(raycastHit.point, 1f);
            }
        }
    }
}