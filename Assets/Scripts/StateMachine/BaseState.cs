using Entity;
using StateMachine.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public abstract class BaseState
    {
        protected IStateSwitcher StateSwitcher;
        public abstract void GetComponents(AliveEntity aliveEntity);
        public abstract void RunState(AliveEntity aliveEntity);
        public abstract void StartState(float time);
        protected void MakeCast(AliveEntity aliveEntity)
        {
            for(int i = 0; i < 3; i++) {
                if(Keyboard.current[(Key) ((int)Key.Digit1 + i)].wasPressedThisFrame) {
                    CastSkillOnIndex(aliveEntity, i);
                }
            }

            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                CastAbilityOnIndex(aliveEntity, 0);
            }
            
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                CastAbilityOnIndex(aliveEntity, 1);
            }
            
            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                CastAbilityOnIndex(aliveEntity, 2);
            }
        }
        
        private void CastSkillOnIndex(AliveEntity aliveEntity, int index)
        {
            var skillCast = StateSwitcher.SwitchState<CastPlayerState>();
            skillCast.CastSkill(index, aliveEntity);
        }

        private void CastAbilityOnIndex(AliveEntity aliveEntity, int index)
        {
            var skillCast = StateSwitcher.SwitchState<CastPlayerState>();
            skillCast.CastAbility(index, aliveEntity);
        }

        
    }
}