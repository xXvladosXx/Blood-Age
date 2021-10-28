namespace DefaultNamespace
{
    using AI;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Attack", menuName = "State/AIAttackState")]
    public class AIAttackState : AttackState
    {
        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            base.OnEnter(characterStateBase, animator, stateInfo);
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
           base.UpdateAbility(characterStateBase, animator, stateInfo);
        }
    }
}