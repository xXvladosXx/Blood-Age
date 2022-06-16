using Entity;
using StateMachine.BaseStates;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    public class TauntEnemyState : TauntBaseState
    {
        private LTDescr _delay;
        private bool _canBeChanged;

        private void StopTaunt()
        {
            LeanTween.cancel(_delay.uniqueId);

            _delay = null;
        }

        public override void RunState(AliveEntity aliveEntity)
        {
        }

        public override void EndState(AliveEntity aliveEntity)
        {
            
        }

        public override bool CanBeChanged => _canBeChanged;

        
        public override void StartState(AliveEntity aliveEntity)
        {
            /*if (time == 0) return;
            if (_delay != null)
            {
                return;
            }
            _canBeChanged = false;

            MoveInOppositeDirection(Entity.transform.forward);
            _delay = LeanTween.delayedCall(time, () =>
            {
                
                _canBeChanged = true;
                StopTaunt();
                StateSwitcher.SwitchState<IdleBaseState>();
            });*/
        }
    }
}