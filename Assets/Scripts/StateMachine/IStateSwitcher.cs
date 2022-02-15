namespace StateMachine
{
    public interface IStateSwitcher
    {
        T SwitchState<T>(float time = 0) where T : BaseState;
        public BaseState GetCurrentState { get; }
    }
}