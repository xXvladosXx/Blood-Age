namespace DefaultNamespace.StateMachine
{
    public interface IStateSwitcher
    {
        T SwitchState<T>() where T : BaseMonoState;
        public BaseMonoState GetCurrentState { get; }
    }
}