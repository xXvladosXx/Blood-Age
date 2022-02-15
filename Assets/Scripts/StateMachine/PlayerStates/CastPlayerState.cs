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
        private AbilityTree _abilityTree;
        private Dictionary<int, ActiveSkill> _indexSkills;

        public override void GetComponents(AliveEntity aliveEntity)
        {
            base.GetComponents(aliveEntity);
            _skillTree = aliveEntity.GetComponent<SkillTree>();
            _abilityTree = aliveEntity.GetComponent<AbilityTree>();
        }

        public void CastSkill(int index, AliveEntity aliveEntity)
        {
            int skillIndex = -1;
            _indexSkills = new Dictionary<int, ActiveSkill>();
            
            foreach (var id in _skillTree.GetActionIds)
            {
                skillIndex++;

                if (id < 0)
                {
                    continue;
                }

                var item = _skillTree.GetActionSkills.FindNecessaryItemInData(id);
                if(item is ActiveSkill activeSkill)
                {
                    _indexSkills.Add(skillIndex, activeSkill);
                }
            }

            if (!_indexSkills.ContainsKey(index))
            {
                StateSwitcher.SwitchState<IdlePlayerState>();
                return;
            }
            if (!_skillTree.CanCastSkill(_indexSkills[index], aliveEntity))
            { 
                StateSwitcher.SwitchState<IdlePlayerState>();
            }
            else
            {
                Movement.enabled = true;
                Movement.Cancel();
            }
        }
        
        public void CastAbility(int index, AliveEntity aliveEntity)
        {
            var casted = _abilityTree.CastAbility(index, aliveEntity);
            if (!casted)
                StateSwitcher.SwitchState<IdlePlayerState>();
        }

        public void ComboCastSkill(AliveEntity aliveEntity, ActiveSkill activeSkill)
        {
            if (activeSkill is AbilitySkill abilitySkill)
            {
                _abilityTree.CastAbility(abilitySkill, aliveEntity);
            }
            else
            {
                _skillTree.ComboSkillCast(activeSkill, aliveEntity);
            }
        }

        public override void RunState(AliveEntity aliveEntity)
        {
            if(Health.IsDead())
                StateSwitcher.SwitchState<IdlePlayerState>();
            
            Movement.Cancel();
        }

        public override void StartState(float time)
        {
        }
    }
}