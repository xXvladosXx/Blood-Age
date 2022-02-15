
using UnityEngine;

namespace StateMachine.BaseStates
{
    public abstract class TauntBaseState : BaseComponentsState, ISwitchable
    {
        protected void MoveInOppositeDirection(Vector3 target)
        {
            Vector3 own = new Vector3(target.x * -100, target.y, target.z *-100);
            ResetAnimatorBools();
            Movement.StartMoveTo(own, 1f);
        }

        public bool CanSwitch()
        {
            return false;
        }
    }
}