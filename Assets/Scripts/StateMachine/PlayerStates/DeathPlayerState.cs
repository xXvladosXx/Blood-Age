using Entity;

namespace StateMachine.PlayerStates
{
    public class DeathPlayerState : BasePlayerState
    {
        public override void RunState(AliveEntity aliveEntity)
        {
            
        }

        public override void StartState(float time)
        {
            Movement.enabled = false;
        }
    }
}