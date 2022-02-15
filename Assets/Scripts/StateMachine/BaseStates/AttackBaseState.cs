using AttackSystem.AttackMechanics;
using Entity;
using InventorySystem;
using States;
using StatsSystem;
using UnityEngine;

namespace StateMachine.BaseStates
{
    public abstract class AttackBaseState : BaseComponentsState, ISwitchable
    {
        public bool CanSwitch()
        {
            return true;
        }
    }
}