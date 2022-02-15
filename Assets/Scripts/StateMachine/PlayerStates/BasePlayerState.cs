using AttackSystem.AttackMechanics;
using Entity;
using InventorySystem;
using StateMachine.BaseStates;
using StatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StateMachine.PlayerStates
{
    public abstract class BasePlayerState : BaseState, ISwitchable
    {
        protected Movement Movement;
        protected PlayerEntity AliveEntity;
        protected StarterAssetsInputs StarterAssetsInputs;
        protected AttackRegister AttackRegister;
        protected Animator Animator;
        protected ItemEquipper ItemEquipper;
        protected Health Health;

        public override void GetComponents(AliveEntity aliveEntity)
        {
            AliveEntity = aliveEntity.GetComponent<PlayerEntity>();
            StateSwitcher = aliveEntity.GetComponent<IStateSwitcher>();
            StarterAssetsInputs = aliveEntity.GetComponent<StarterAssetsInputs>();
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

        public bool CanSwitch()
        {
            return true;
        }
    }
}