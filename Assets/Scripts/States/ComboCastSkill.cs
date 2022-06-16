using Entity;
using SkillSystem;
using SkillSystem.SkillNodes;
using SkillSystem.Skills;
using StateMachine;
using StateMachine.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States
{
    [CreateAssetMenu(menuName = "State/ComboCastSkill")]
    public class ComboCastSkill : StateData
    {
        [SerializeField] private float _startTime;
        [SerializeField] private float _endTime;
        [SerializeField] private SkillNode _requiredSkill;

        private IStateSwitcher _playerStateManager;
        private bool _wasApplied;
        private bool _haveRequiredSkill = true;
        private SkillTree _skillTree;
        
        private static readonly int SkillContinueCombo = Animator.StringToHash("SkillContinueCombo");

        public override void OnEnter(AnimatorState characterStateAnimator, Animator animator,
            AnimatorStateInfo stateInfo)
        {
            ComboKeySpawner.Instance.ResetChainKey();
            _playerStateManager = animator.GetComponent<IStateSwitcher>();
            _skillTree = animator.GetComponent<SkillTree>();
            _wasApplied = false;

            if (_requiredSkill == null)
            {
                _haveRequiredSkill = true;
                return;
            }
            _haveRequiredSkill = (_skillTree.GetKnownSkills.HasItemInInventory(_requiredSkill.Data));
        }

        public override void UpdateAbility(AnimatorState characterStateAnimator, Animator animator,
            AnimatorStateInfo stateInfo)
        {
            if(!_haveRequiredSkill) return;
            if (_wasApplied)
            {
                ComboKeySpawner.Instance.ResetChainKey();
                return;
            }
            
            _playerStateManager.SwitchState<CastPlayerState>();
            ComboKeySpawner.Instance.SetChainKey();
            
            if (Keyboard.current[Key.Space].wasPressedThisFrame)
            {
                if (stateInfo.normalizedTime >= _startTime)
                {
                    _wasApplied = true;
                    animator.SetBool(SkillContinueCombo, true);
                    ComboKeySpawner.Instance.ResetChainKey();
                }
            }

            if (stateInfo.normalizedTime >= _endTime)
            {
                ComboKeySpawner.Instance.ResetChainKey();
            }
        }

        public override void OnExit(AnimatorState characterStateAnimator, Animator animator,
            AnimatorStateInfo stateInfo)
        {
            animator.SetBool(SkillContinueCombo, false);
            ComboKeySpawner.Instance.ResetChainKey();
            if(_wasApplied) return;
        }
    }
}