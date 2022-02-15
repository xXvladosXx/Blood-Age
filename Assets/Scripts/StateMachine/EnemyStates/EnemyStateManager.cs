using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using MouseSystem;
using StateMachine.BaseStates;
using StateMachine.PlayerStates;
using UI.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace StateMachine.EnemyStates
{
    public class EnemyStateManager : MonoBehaviour, IStateSwitcher, IRaycastable
    {
        [SerializeField] private List<BaseState> _allStates;
        [SerializeField] private StateDistanceConfiguration _stateDistanceConfiguration;
        [SerializeField] private Transform _pathToPatrol;
        
        private BaseState _currentBaseState;
        private AliveEntity _aliveEntity;
        
        private void Awake()
        {
            _aliveEntity = GetComponent<AliveEntity>();

            if (_stateDistanceConfiguration == null)
            {
                _stateDistanceConfiguration = Resources.Load<StateDistanceConfiguration>("DistanceConfigurationDefaultEnemy");
            }
            

            _allStates = new List<BaseState>
            {
                new IdleEnemyState(_pathToPatrol, _stateDistanceConfiguration),
                new ChaseEnemyState(_stateDistanceConfiguration),
                new AttackEnemyState(_stateDistanceConfiguration),
                new StunEnemyState(),
                new TauntEnemyState()
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

        private void Update()
        {
            _currentBaseState.RunState(_aliveEntity);
        }

        public T SwitchState<T>(float time) where T : BaseState
        {
            var state = _allStates.FirstOrDefault(s => s is T);
            if (state == null) return null;
            if (_currentBaseState is ISwitchable curState && state is ISwitchable nextState)
            {
                if (!curState.CanSwitch() && !nextState.CanSwitch())
                {
                    return null;
                }
            }
            
            state.StartState(time);
            _currentBaseState = state;

            return (T) state;
        }

        public BaseState GetCurrentState => _currentBaseState;
        public CursorType GetCursorType() => CursorType.Combat;
        public void ClickAction()
        {
            HealthBarEntity.Instance.ShowHealth(_aliveEntity);
        }

        public bool HandleRaycast(PlayerEntity player)
        {
            return !_aliveEntity.GetHealth.IsDead();
        }
    }
}