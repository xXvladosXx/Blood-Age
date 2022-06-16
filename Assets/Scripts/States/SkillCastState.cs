using Entity;
using SkillSystem;
using SkillSystem.SkillNodes;
using SkillSystem.Skills;
using StateMachine;
using StateMachine.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace States
{
    [CreateAssetMenu(menuName = "State/CastSkillContiniue")]
    public class SkillCastState : StateData
    {
        [SerializeField] private ActiveSkill _activeSkill;
        [SerializeField] private float _startTime;
        [SerializeField] private float _endTime;
        [SerializeField] private MouseButton _mouseButton = MouseButton.Back;
        [SerializeField] private SkillNode _requiredSkill;

        private IStateSwitcher _playerStateManager;
        private AliveEntity _aliveEntity;
        private bool _wasApplied;
        private bool _canMakeCombo;
        private SkillTree _skillTree;
        
        private static readonly int SkillCombo = Animator.StringToHash("SkillCombo");
        private static readonly int Canceled = Animator.StringToHash("Canceled");

        public override void OnEnter(AnimatorState characterStateAnimator, Animator animator,
            AnimatorStateInfo stateInfo)
        {
            _playerStateManager = animator.GetComponent<IStateSwitcher>();
            _aliveEntity = animator.GetComponent<AliveEntity>();
            _skillTree = _aliveEntity.GetComponent<SkillTree>();
            _wasApplied = false;
            ComboKeySpawner.Instance.ResetComboKey();

            _playerStateManager.SwitchState<CastPlayerState>();

            if (_requiredSkill == null)
            {
                _canMakeCombo = true;
                return;
            }

            _canMakeCombo = (_skillTree.GetKnownSkills.HasItemInInventory(_requiredSkill.Data));
        }

        public override void UpdateAbility(AnimatorState characterStateAnimator, Animator animator,
            AnimatorStateInfo stateInfo)
        {
            if(_canMakeCombo == false) return;
            _playerStateManager.SwitchState<CastPlayerState>();
            if (stateInfo.normalizedTime >= _startTime && stateInfo.normalizedTime <= _endTime && !_wasApplied)
            {
                if(_mouseButton != MouseButton.Back)
                    ComboKeySpawner.Instance.SetComboKey();
            }
            else
            {
                ComboKeySpawner.Instance.ResetComboKey();
            }
            
            if(_wasApplied) return;
            
            if (MouseButton.Back == _mouseButton)
            {
                ManualApplySkill(animator, stateInfo);
                ComboKeySpawner.Instance.ResetComboKey();
                return;
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (!(stateInfo.normalizedTime >= _startTime) || !(stateInfo.normalizedTime <= _endTime)) return;
                animator.SetBool(SkillCombo, true);
                ComboKeySpawner.Instance.ResetComboKey();
                
                if (_activeSkill == null) return;
                _playerStateManager.SwitchState<CastPlayerState>().ComboCastSkill(_aliveEntity, _activeSkill);
                _wasApplied = true;
            }
        }

        private void ManualApplySkill(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!(stateInfo.normalizedTime >= _startTime)) return;
            if (_activeSkill == null) return;

            _playerStateManager.SwitchState<CastPlayerState>().ComboCastSkill(_aliveEntity, _activeSkill);
            animator.SetBool(SkillCombo, false);
            _wasApplied = true;
        }

        public override void OnExit(AnimatorState characterStateAnimator, Animator animator,
            AnimatorStateInfo stateInfo)
        {
            _playerStateManager.SwitchState<IdlePlayerState>();
            ComboKeySpawner.Instance.ResetComboKey();
            
            animator.SetBool(SkillCombo, false);
            animator.SetBool(Canceled, false);
        }
    }
}