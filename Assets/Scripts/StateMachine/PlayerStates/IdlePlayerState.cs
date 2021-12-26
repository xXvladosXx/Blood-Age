namespace DefaultNamespace.StateMachine.PlayerStates
{
    using DefaultNamespace.Entity;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class IdlePlayerState : BaseMonoState
    {
        protected static readonly int ForceTransition = Animator.StringToHash("ForceTransition");
        protected static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Roll = Animator.StringToHash("Roll");
        private static readonly int Canceled = Animator.StringToHash("Canceled");

        private StarterAssetsInputs _starterAssetsInputs;
        private AttackRegistrator _attackRegistrator;
        private Movement _movement;
        private Animator _animator;

        public override void StartState(AliveEntity aliveEntity)
        {
            _starterAssetsInputs = aliveEntity.GetComponent<StarterAssetsInputs>();
            _attackRegistrator = aliveEntity.GetComponentInChildren<AttackRegistrator>();
            _movement = aliveEntity.GetComponent<Movement>();
            _stateSwitcher = aliveEntity.GetComponent<IStateSwitcher>();
            _animator = aliveEntity.GetComponent<Animator>();
        }

        public override void RunState(AliveEntity aliveEntity)
        {
            if (_starterAssetsInputs.enabled == false) return;
            if (aliveEntity.GetHealth.IsDead()) return;
            
            Debug.Log("isInIdle");
            PlayerCastSkillInput(aliveEntity);
        }

        private void PlayerCastSkillInput(AliveEntity aliveEntity)
        {
            if(_stateSwitcher.GetCurrentState is CastPlayerState) return;
            
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                var skillCast = _stateSwitcher.SwitchState<CastPlayerState>();
                Debug.Log("casted first");
                skillCast.CastSkill(0, aliveEntity);
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                var skillCast = _stateSwitcher.SwitchState<CastPlayerState>();
                Debug.Log("casted first");
                skillCast.CastSkill(1, aliveEntity);
            }
        }

       
    }
}