/*using System.Collections.Generic;
using Entity;
using InventorySystem;
using SkillSystem;
using SkillSystem.MainComponents;
using SkillSystem.Skills;
using UI.Inventory;
using UnityEngine;

namespace UI.Skill
{
    public class SkillPanel : Panel
    {
        [SerializeField] private List<Item> _activeSkills = new List<Item>();

        private SkillTree _skillTree;
        private SkillDisplay[] _skillDisplays;
        private UserInterface _userInterface;

        /*private void SetDataToDisplay()
        {
            for (int i = 0; i < _skillDisplays.Length; i++)
            {
                if (i > _skillTree.GetActionSkills.GetAllItems().Count - 1) break;

                if (_skillTree.GetActionSkills.FindNecessaryItemInData(_skillTree.GetActionSkills.GetAllItems()[i]) is
                    ActiveSkill activeSkill)
                {
                    _skillDisplays[i].SetSkill(activeSkill);
                }
                else
                {
                    _skillDisplays[i].ResetSkill();
                }
            }
        }#1#

        private void Update()
        {
            foreach (var skillDisplay in _skillDisplays)
            {
                skillDisplay.SetSkills(_skillTree.GetSkillsInCooldown);
            }
        }

        public override void Initialize(AliveEntity aliveEntity)
        {
            _skillTree = aliveEntity.GetComponent<SkillTree>();
            _userInterface = GetComponent<UserInterface>();

            foreach (var skillNode in _skillTree.GetActionItems)
            {
                _activeSkills.Add(skillNode);

                if (_userInterface.GetItemContainer.FindSlotInInventory(new ItemData(skillNode)) != null) continue;

                _userInterface.GetItemContainer.AddItem(new ItemData(skillNode), 1);
            }

            // _skillTree.OnSkillsChanged += SetDataToDisplay;
            // _skillDisplays = GetComponentsInChildren<SkillDisplay>();
            //
            // SetDataToDisplay();
        }
    }
}*/