using Entity;

namespace StateMachine.BaseStates
{
    public abstract class DeathBaseState : BaseComponentsState, ISwitchable
    {
        public bool CanSwitch()
        {
            return false;
        }
    }
}