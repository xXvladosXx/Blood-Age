using System;
using System.Collections.Generic;
using System.Linq;

using Entity;
using StateMachine.BaseStates;
using UnityEngine;

namespace StateMachine.PlayerStates
{
    public class PlayerStateManager : MonoBehaviour, IStateSwitcher
    {
        [SerializeField] private List<BaseState> _allStates;

        private BaseState _currentBaseState;
        private AliveEntity _aliveEntity;
        public static event Action<AliveEntity> OnPlayerSpawn = delegate{  };
        public event Action<AliveEntity> OnPlayerDie;

        private void Awake()
        {
            _aliveEntity = GetComponent<AliveEntity>();

            _allStates = new List<BaseState>
            {
                new IdlePlayerState(),
                new AttackPlayerState(),
                new CastPlayerState(),
                new DialoguePlayerState(),
                new PickingPlayerState(),
                new DeathPlayerState(),
                new TeleportPlayerState()
            };

            _currentBaseState = _allStates[0];
            OnPlayerSpawn(_aliveEntity);
        }

        private void Start()
        {
            foreach (var state in _allStates)
            {
                state.GetComponents(_aliveEntity);
            }
        }

        private void Update()
        {
            print(_currentBaseState);

            _currentBaseState.RunState(_aliveEntity);
        }

        public T SwitchState<T>(float time) where T : BaseState
        {
            var state = _allStates.FirstOrDefault(s => s is T);
            _currentBaseState = state;
            
            return (T)state;
        }

        public bool CanSave()
        {
            if (_aliveEntity.Targets.Count == 0)
                return true;

            if (_currentBaseState is DeathPlayerState)
                return false;

            if (_currentBaseState is TeleportPlayerState)
                return false;

            return false;
        }
        public BaseState GetCurrentState => _currentBaseState;
    }
}