using System;
using System.Collections.Generic;
using System.Linq;

using Entity;
using SaveSystem;
using StateMachine.BaseStates;
using UnityEngine;

namespace StateMachine.PlayerStates
{
    public class PlayerStateManager : MonoBehaviour, IStateSwitcher
    {
        [SerializeField] private List<BaseState> _allStates;

        private BaseState _currentBaseState;
        private static PlayerEntity _aliveEntity;
        public static PlayerEntity GetPlayer => _aliveEntity;

        private void Awake()
        {
            _aliveEntity = GetComponent<PlayerEntity>();

            _allStates = new List<BaseState>
            {
                new IdlePlayerState(),
                new AttackPlayerState(),
                new CastPlayerState(),
                new DialoguePlayerState(),
                new PickingPlayerState(),
                new DeathPlayerState(),
                new TeleportPlayerState(), 
                new ShopPlayerState()
            };

            _currentBaseState = _allStates[0];
        }

        private void Start()
        {
            foreach (var state in _allStates)
            {
                state.GetComponents(_aliveEntity);
            }
        }

        private void FixedUpdate()
        {
            _currentBaseState.RunState(_aliveEntity);
        }

        public T SwitchState<T>() where T : BaseState
        {
            var state = _allStates.FirstOrDefault(s => s is T);
            if (state != null)
            {
                _currentBaseState.EndState(_aliveEntity);
                _currentBaseState = state;
                state.StartState(_aliveEntity);
                return (T) state;
            }

            return null;
        }

        public bool CanSave()
        {
            if (_currentBaseState is DeathBaseState)
                return false;

            if (_currentBaseState is TeleportPlayerState)
                return false;
            
            return _aliveEntity.Targets.Count == 0;
        }
        public BaseState GetCurrentState => _currentBaseState;
        
    }
}