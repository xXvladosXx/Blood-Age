using System;
using Entity;
using StateMachine.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    [Serializable]
    public abstract class BaseState
    {
        protected IStateSwitcher StateSwitcher;
        public abstract void GetComponents(AliveEntity aliveEntity);
        public abstract void RunState(AliveEntity aliveEntity);
        public abstract void StartState(AliveEntity aliveEntity);

        public abstract void EndState(AliveEntity aliveEntity);

        public abstract bool CanBeChanged { get; }
        }
}