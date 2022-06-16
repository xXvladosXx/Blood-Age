using System;
using Entity;
using StateMachine.BaseStates;
using UnityEngine;

namespace StateMachine.PlayerStates
{
    [Serializable]
    public class DeathPlayerState : DeathBaseState
    {
        private PlayerInputs _playerInputs;
        public override void GetComponents(AliveEntity aliveEntity)
        {
            base.GetComponents(aliveEntity);
            _playerInputs = aliveEntity.GetComponent<PlayerInputs>();
        }

        public override void RunState(AliveEntity aliveEntity)
        {
            if (Health.GetCurrentHealth > 0)
            {
                StateSwitcher.SwitchState<IdleBaseState>();
            }
        }

        public override void StartState(AliveEntity aliveEntity)
        {
            Movement.enabled = false;
            _playerInputs.enabled = false;
        }

        public override void EndState(AliveEntity aliveEntity)
        {
            Movement.enabled = true;
            _playerInputs.enabled = true;
        }

        public override bool CanBeChanged => false;

    }
}