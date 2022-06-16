using AttackSystem.AttackMechanics;
using Entity;
using InventorySystem;
using StateMachine.BaseStates;
using StatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StateMachine.PlayerStates
{
    public abstract class BasePlayerState : BaseState
    {
        protected Movement Movement;
        protected PlayerEntity PlayerEntity;
        protected PlayerInputs PlayerInputs;
        protected AttackRegister AttackRegister;
        protected Animator Animator;
        protected ItemEquipper ItemEquipper;
        protected Health Health;

        public override void GetComponents(AliveEntity aliveEntity)
        {
            PlayerEntity = aliveEntity.GetComponent<PlayerEntity>();
            StateSwitcher = aliveEntity.GetComponent<IStateSwitcher>();
            PlayerInputs = aliveEntity.GetComponent<PlayerInputs>();
            Animator = aliveEntity.GetComponent<Animator>();
            Movement = aliveEntity.GetComponent<Movement>();
            AttackRegister = aliveEntity.GetAttackRegister;
            ItemEquipper = aliveEntity.GetComponent<ItemEquipper>();
            Health = aliveEntity.GetHealth;
        }

        protected bool PointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

    }
}