using Entity;
using StateMachine.BaseStates;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    public class TauntEnemyState : TauntBaseState
    {
        private LTDescr _delay;
        private void StopTaunt()
        {
            LeanTween.cancel(_delay.uniqueId);

            _delay = null;
        }

        public override void RunState(AliveEntity aliveEntity)
        {
        }

        public override void StartState(float time)
        {
            if (time == 0) return;
            if (_delay != null)
            {
                return;
            }

            MoveInOppositeDirection(Entity.transform.forward);
            _delay = LeanTween.delayedCall(time, () =>
            {
                StopTaunt();
                StateSwitcher.SwitchState<IdleBaseState>();
            });
        }
    }
}