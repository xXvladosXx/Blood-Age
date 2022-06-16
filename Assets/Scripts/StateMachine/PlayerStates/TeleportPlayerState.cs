using Entity;
using SceneSystem;
using UnityEngine;

namespace StateMachine.PlayerStates
{
    public class TeleportPlayerState : BasePlayerState
    {
        public override void RunState(AliveEntity aliveEntity)
        {
           
        }

        public override void EndState(AliveEntity aliveEntity)
        {
            
        }

        public override bool CanBeChanged => false;

        public override void StartState(AliveEntity aliveEntity)
        {
            PlayerInputs.enabled = false;
        }
    }
}