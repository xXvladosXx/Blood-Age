using StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace States
{
    [CreateAssetMenu (menuName = "State/Unmovable")]
    public class UnmovableState : StateData
    {
        private Movement _movement;

        public override void OnEnter(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            _movement = animator.GetComponent<Movement>();
            _movement.EnableMovement(false);
        }

        public override void UpdateAbility(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void OnExit(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            _movement.EnableMovement(true);
        }
    }
}