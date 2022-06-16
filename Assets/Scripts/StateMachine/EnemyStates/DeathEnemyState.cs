using Entity;
using StateMachine.BaseStates;

namespace StateMachine.EnemyStates
{
    public class DeathEnemyState : DeathBaseState
    {
        public override void RunState(AliveEntity aliveEntity)
        {
            
        }

        public override void StartState(AliveEntity aliveEntity)
        {
        }

        public override void EndState(AliveEntity aliveEntity)
        {
            
        }

        public override bool CanBeChanged => false;

    }
}