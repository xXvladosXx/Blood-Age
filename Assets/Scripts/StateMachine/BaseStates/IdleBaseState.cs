using AttackSystem.AttackMechanics;
using Entity;
using InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StateMachine.BaseStates
{
    public abstract class IdleBaseState : BaseComponentsState, ISwitchable
    {
        protected bool PointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        public bool CanSwitch()
        {
            return true;
        }
    }
}