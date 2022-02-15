using Entity;

namespace StateMachine.PlayerStates
{
    public class TeleportPlayerState : BasePlayerState
    {
        public override void RunState(AliveEntity aliveEntity)
        {
            
        }

        public override void StartState(float time)
        {
            StarterAssetsInputs.enabled = false;
        }
    }
}