using System;
using System.Collections.Generic;
using Entity;
using SkillSystem;
using SkillSystem.Skills;

namespace UI.Ability
{
    public class SkillPanelAction : SlotPanelAction
    {
        private SkillTree _skillTree;
        private List<SlotHandler> _slotHandlers = new List<SlotHandler>();
        public override void Initialize(AliveEntity aliveEntity)
        {
            _skillTree = aliveEntity.GetComponent<SkillTree>();
            SlotChoose = GetComponentInChildren<SlotChoose>();
            KnownItems = _skillTree.GetKnownSkills;
            ActionItems = _skillTree.GetActionSkills;
            
            CreateSlots();
            
            foreach (var key in SlotOnUI.Keys)
            {
                _slotHandlers.Add(key);
            }
            
            SlotChoose.OnSlotChange += CalculateCooldown;
        }

        protected override void Update()
        {
            base.Update();
            
            foreach (var actionItem in ActionItems.GetInventoryItems())
            {
                if (!(actionItem is ActiveSkill activeSkill)) continue;
                if (!_skillTree.GetSkillsInCooldown.ContainsKey(activeSkill)) continue;
                
                foreach (var slotHandler in _slotHandlers)
                {
                    slotHandler.StartCooldown(activeSkill, _skillTree.GetSkillsInCooldown[activeSkill]);
                }
            }
        }
        
        private void CalculateCooldown()
        {
            UpdateSlots();
        }
    }
}