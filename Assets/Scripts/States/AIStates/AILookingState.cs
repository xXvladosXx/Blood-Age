namespace DefaultNamespace
{
    using UnityEngine;

    [CreateAssetMenu (fileName = "AILooking", menuName = "State/AILookingState")]
    public class AILookingState : IdleMovementState
    {
        private static readonly int Transition = Animator.StringToHash("ForceTransition");

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (_attackRegistrator.AttackData.Target != null)
            {
                animator.SetBool(Transition, true);
            }
        }
    }
}