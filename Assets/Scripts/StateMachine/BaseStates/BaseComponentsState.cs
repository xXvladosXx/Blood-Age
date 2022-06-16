using System;
using AttackSystem.AttackMechanics;
using Entity;
using InventorySystem;
using StatsSystem;
using UnityEngine;

namespace StateMachine.BaseStates
{
    [Serializable]
    public abstract class BaseComponentsState : BaseState
    {
        protected Health Health;
        protected Movement Movement;
        protected ItemEquipper ItemEquipper;
        protected AliveEntity Entity;
        protected FindStats FindStat;
        protected AttackRegister AttackRegister;
        protected Animator Animator;
        private static readonly int Attack = Animator.StringToHash("Attack");

        public override void GetComponents(AliveEntity aliveEntity)
        {
            StateSwitcher = aliveEntity.GetComponent<IStateSwitcher>();
            Health = aliveEntity.GetHealth;
            Movement = aliveEntity.GetComponent<Movement>();
            ItemEquipper = aliveEntity.GetItemEquipper;
            AttackRegister = aliveEntity.GetAttackRegister;
            Entity = aliveEntity;
            FindStat = aliveEntity.GetComponent<FindStats>();
            Animator = aliveEntity.GetComponent<Animator>();
        }
        
        protected void ResetAnimatorBools()
        {
            Animator.SetBool(Attack, false);
        }
    }
}