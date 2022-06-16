using AttackSystem.AttackMechanics;
using Entity;
using InventorySystem;
using States;
using StatsSystem;
using UnityEngine;

namespace StateMachine.BaseStates
{
    public abstract class AttackBaseState : BaseComponentsState
    {
        protected static readonly int Attack = Animator.StringToHash("Attack");
        protected static readonly int ForceTransition = Animator.StringToHash("ForceTransition");
        public override void StartState(AliveEntity aliveEntity)
        {
            Animator.SetBool(ForceTransition, false);
            Entity.OnDied += entity => StateSwitcher.SwitchState<IdleBaseState>();
        }
        
        public override void EndState(AliveEntity aliveEntity)
        {
            
        }
    }
}