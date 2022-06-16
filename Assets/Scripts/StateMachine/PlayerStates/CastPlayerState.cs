using System.Collections.Generic;
using Entity;
using SkillSystem;
using SkillSystem.Skills;
using States;
using UnityEngine;

namespace StateMachine.PlayerStates
{
    public class CastPlayerState : BasePlayerState
    {
        private SkillTree _skillTree;

        public override void GetComponents(AliveEntity aliveEntity)
        {
            base.GetComponents(aliveEntity);
            _skillTree = aliveEntity.GetComponent<SkillTree>();
        }

        public bool CastSkill(int index, AliveEntity aliveEntity)
        {
            if (_skillTree.CanCastSkill(index))
            {
                _skillTree.CastSkill(index, aliveEntity);
                return true;
            }

            StateSwitcher.SwitchState<IdlePlayerState>();
            Movement.enabled = true;
            Movement.Cancel();
            return false;
        }

        // public void CastAbility(int index, AliveEntity aliveEntity)
        // {
        //     var casted = _abilityTree.CastAbility(index, aliveEntity);
        //     if (!casted)
        //         StateSwitcher.SwitchState<IdlePlayerState>();
        // }

        public void ComboCastSkill(AliveEntity aliveEntity, ActiveSkill activeSkill)
        {
            _skillTree.ComboSkillCast(activeSkill, aliveEntity);

            // if (activeSkill is AbilitySkill abilitySkill)
            // {
            //     _abilityTree.CastAbility(abilitySkill, aliveEntity);
            // }
            // else
            // {
            //}
        }

        public override void RunState(AliveEntity aliveEntity)
        {
            Movement.Cancel();
        }

        public override void EndState(AliveEntity aliveEntity)
        {
            
        }

        public override bool CanBeChanged => true;

        public override void StartState(AliveEntity aliveEntity)
        {
            PlayerEntity.OnDied += entity => StateSwitcher.SwitchState<IdlePlayerState>();
        }
    }
}