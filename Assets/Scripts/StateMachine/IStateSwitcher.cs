namespace StateMachine
{
    public interface IStateSwitcher
    {
        T SwitchState<T>() where T : BaseState;
        public BaseState GetCurrentState { get; }
    }
}