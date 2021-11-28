namespace DefaultNamespace
{
    using DefaultNamespace.SkillSystem.SkillNodes;
    using global::SkillSystem;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [CreateAssetMenu (menuName = "State/CastSkill")]
    public class CastingSkill : StateData
    {
        private SkillNode[] _skills;
        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            _skills = animator.GetComponent<SkillBuilder>().GetSkillNodes;
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                CasteSkill(0, animator);
            
            if(Keyboard.current.digit1Key.wasPressedThisFrame)
                CasteSkill(1, animator);
        }

        private void CasteSkill(int index, Animator animator)
        {
            _skills[index].ApplySkill(animator.gameObject);
        }
        
        public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

    }
}