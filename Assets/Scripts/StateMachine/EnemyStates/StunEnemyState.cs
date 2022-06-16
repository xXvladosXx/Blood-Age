using Entity;
using StateMachine.BaseStates;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    public class StunEnemyState : StunBaseState
    {
        private LTDescr _delay;
        private bool _canBeChanged;

        private static readonly int Stun = Animator.StringToHash("StopStun");

        private void StopStun()
        {
            Animator.SetBool(Stun, true);

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

            Animator.Play($"Stun");
            Movement.Cancel();
            Animator.SetBool(Stun, false);
            _canBeChanged = false;

            _delay = LeanTween.delayedCall(time, () =>
            {
                _canBeChanged = true;
                StopStun();
                Animator.Play($"Idle Walk Run Blend");
                StateSwitcher.SwitchState<IdleBaseState>();
            });
        }*/
        }
    }
}