namespace DefaultNamespace
{
    using UnityEngine;
    using UnityEngine.AI;

    [CreateAssetMenu (menuName = "State/Death")]
    public class DeathState : StateData
    {
        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.GetComponent<Movement>().Cancel();
            
            Destroy(animator.GetComponent<Movement>());
            Destroy(animator.GetComponent<Collider>());
            Destroy(animator.GetComponent<NavMeshAgent>());
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
    }
}