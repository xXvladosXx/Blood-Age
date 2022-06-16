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
    [RequireComponent(typeof(EnemyEntity))]
    public class EnemyStateManager : MonoBehaviour, IStateSwitcher, IRaycastable
    {
        [SerializeField] protected List<BaseState> AllStates;
        [SerializeField] protected StateDistanceConfiguration StateDistanceConfiguration;
        [SerializeField] protected Transform PathToPatrol;

        protected BaseState CurrentBaseState;
        protected AliveEntity AliveEntity;

        protected virtual void Awake()
        {
            AliveEntity = GetComponent<EnemyEntity>();

            if (StateDistanceConfiguration == null)
            {
                StateDistanceConfiguration =
                    Resources.Load<StateDistanceConfiguration>("DistanceConfigurationDefaultEnemy");
            }


            AllStates = new List<BaseState>
            {
                new IdleEnemyState(PathToPatrol, StateDistanceConfiguration),
                new ChaseEnemyState(StateDistanceConfiguration),
                new AttackEnemyState(StateDistanceConfiguration),
                new StunEnemyState(),
                new TauntEnemyState()
            };
            CurrentBaseState = AllStates[0];
        }

        private void Start()
        {
            foreach (var state in AllStates)
            {
                state.GetComponents(AliveEntity);
            }
        }

        private void Update()
        {
            CurrentBaseState.RunState(AliveEntity);
        }

        public T SwitchState<T>() where T : BaseState
        {
            var state = AllStates.FirstOrDefault(s => s is T);
            if (state == null) return null;
            if(!CurrentBaseState.CanBeChanged && !(state is DeathBaseState))
                return null;

            CurrentBaseState.EndState(AliveEntity);
            state.StartState(AliveEntity);
            CurrentBaseState = state;

            return (T) state;
        }

        public BaseState GetCurrentState => CurrentBaseState;
        public CursorType GetCursorType() => CursorType.Combat;


        public bool HandleRaycast(PlayerEntity player)
        {
            return !AliveEntity.GetHealth.IsDead();
        }
    }
}