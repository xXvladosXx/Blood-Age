using UnityEngine;

namespace States
{
    public abstract class StateData : ScriptableObject
    {
        public abstract void OnEnter(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo);
        public abstract void UpdateAbility(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo);
        public abstract void OnExit(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo);
    }
}