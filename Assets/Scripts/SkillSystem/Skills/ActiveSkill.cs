namespace SkillSystem.MainComponents
{
    using DefaultNamespace;
    using DefaultNamespace.Entity;
    using DefaultNamespace.SkillSystem;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using DefaultNamespace.StateMachine;
    using DefaultNamespace.StateMachine.PlayerStates;
    using DefaultNamespace.UI.ButtonClickable;
    using SkillSystem.MainComponents.EffectApplyingSkills;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Skill/ActiveSkill", order = 0)]
    
    public class ActiveSkill : SkillNode
    {
        [SerializeField] private Sprite _skillSprite;
        [SerializeField] private Targeting _targeting;
        [SerializeField] private Filtering[] _filtering;
        [SerializeField] private EffectApplying[] _effectApplying;

        private CharacteristicBonus[] _playerPassiveSkillBonus;

        private Animator _animator;
        private AliveEntity _user;
        private static readonly int Canceled = Animator.StringToHash("Canceled");

        public Sprite GetSkillSprite => _skillSprite;

        public override void ApplySkill(AliveEntity user)
        {
            if(_targeting == null) return;

            _animator = user.GetComponent<Animator>();
            SkillData skillData = new SkillData(user);
            _user = user;
            _targeting.StartTargeting(skillData, () => AcquireTarget(skillData), Cancel);
        }

        private void Cancel()
        {
            _animator.SetBool(Canceled, true);

            ExecuteState();
        }

        private void ExecuteState()
        {
            var state = _user.GetComponent<IStateSwitcher>().GetCurrentState;

            if (state is CastPlayerState castPlayerState)
            {
                castPlayerState.SwitchToIdle();
            }
        }

        private void AcquireTarget(SkillData skillData)
        {
            if(skillData.IsCancelled) return;

            foreach (var filtering in _filtering)
            {
                skillData.Target = filtering.StartFiltering(skillData.Target);
            }

            foreach (var effectApplying in _effectApplying)
            {
                effectApplying.Effect(skillData, Finished);
            }
        }

        private void Finished()
        {
            Debug.Log("effect finished");
        }
    }
}