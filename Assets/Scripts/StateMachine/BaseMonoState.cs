namespace DefaultNamespace.StateMachine
{
    using DefaultNamespace.Entity;
    using UnityEngine;

    public abstract class BaseMonoState
    {
        protected IStateSwitcher _stateSwitcher;
        public abstract void StartState(AliveEntity aliveEntity);
        public abstract void RunState(AliveEntity aliveEntity);
    }
}