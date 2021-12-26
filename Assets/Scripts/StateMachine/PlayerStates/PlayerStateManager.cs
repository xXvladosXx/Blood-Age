namespace DefaultNamespace.StateMachine.PlayerStates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace.Entity;
    using DefaultNamespace.StateMachine.BaseStates;
    using UnityEngine;

    public class PlayerStateManager : MonoBehaviour, IStateSwitcher
    {
        [SerializeField] private List<BaseMonoState> _allStates;

        private BaseMonoState _currentBaseState;
        private AliveEntity _aliveEntity;

        private void Awake()
        {
            _aliveEntity = GetComponent<AliveEntity>();
            
            _allStates = new List<BaseMonoState>
            {
                new IdlePlayerState(),
                new AttackPlayerState(),
                new CastPlayerState()
            };

            foreach (var state in _allStates)
            {
                state.StartState(_aliveEntity);
            }

            _currentBaseState = _allStates[0];
        }

        private void Update()
        {
            _currentBaseState.RunState(_aliveEntity);
        }

        public T SwitchState<T>() where T : BaseMonoState
        {
            var state = _allStates.FirstOrDefault(s => s is T);
            _currentBaseState = state;

            if (state is ISwitchListener listener)
            {
                listener.OnSwitch();
            }

            return (T)state;
        }

        public BaseMonoState GetCurrentState => _currentBaseState;
    }
}