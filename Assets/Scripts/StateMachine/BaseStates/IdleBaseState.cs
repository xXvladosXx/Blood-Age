using System;
using AttackSystem.AttackMechanics;
using Entity;
using InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StateMachine.BaseStates
{
    [Serializable]
    public abstract class IdleBaseState : BaseComponentsState
    {
        public event Action OnAlive;
        
        protected static readonly int ForceTransition = Animator.StringToHash("ForceTransition");
        private static readonly int Attack = Animator.StringToHash("Attack");

        public override void StartState(AliveEntity aliveEntity)
        {
            Animator.Play("Idle Walk Run Blend");
            
            Entity.OnDied += entity => StateSwitcher.SwitchState<DeathBaseState>();
            OnAlive?.Invoke();
        }
        
        public override void EndState(AliveEntity aliveEntity)
        {
        }
    }
}