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
        [SerializeField] private Key _key;
        [SerializeField] private SkillNode _requiredSkill;

        private IStateSwitcher _playerStateManager;
        private bool _wasApplied;
        private bool _haveRequiredSkill = true;
        private SkillTree _skillTree;
        
        private static readonly int SkillContinueCombo = Animator.StringToHash("SkillContinueCombo");

        public override void OnEnter(AnimatorState characterStateAnimator, Animator animator,
            AnimatorStateInfo stateInfo)
        {
            ComboKeySpawner.Instance.ResetKeyStart();
            _playerStateManager = animator.GetComponent<IStateSwitcher>();
            _skillTree = animator.GetComponent<SkillTree>();
            _wasApplied = false;

            if (_requiredSkill == null)
            {
                _haveRequiredSkill = true;
                _key = ComboKeySpawner.Instance.GetKeyFromNumber();
                return;
            }
            _haveRequiredSkill = (_skillTree.GetKnownSkills.HasItemInInventory(_requiredSkill.Data));

            if (_haveRequiredSkill)
            {
                _key = ComboKeySpawner.Instance.GetKeyFromNumber();
            }
        }

        public override void UpdateAbility(AnimatorState characterStateAnimator, Animator animator,
            AnimatorStateInfo stateInfo)
        {
            if(!_haveRequiredSkill) return;
            
            _playerStateManager.SwitchState<CastPlayerState>();

            if (Keyboard.current[_key].wasPressedThisFrame)
            {
                if (stateInfo.normalizedTime >= _startTime)
                {
                    _wasApplied = true;
                    animator.SetBool(SkillContinueCombo, true);
                    ComboKeySpawner.Instance.ResetKeyStart();
                }
            }

            if (stateInfo.normalizedTime >= _endTime)
            {
                ComboKeySpawner.Instance.ResetKeyStart();
            }
        }

        public override void OnExit(AnimatorState characterStateAnimator, Animator animator,
            AnimatorStateInfo stateInfo)
        {
            animator.SetBool(SkillContinueCombo, false);
            ComboKeySpawner.Instance.ResetKeyStart();
            if(_wasApplied) return;
        }
    }
}