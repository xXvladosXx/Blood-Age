using System.Collections.Generic;
using Entity;
using InventorySystem;
using UnityEngine;

namespace SkillSystem.SkillNodes
{
    public abstract class SkillNode : Item
    {
        [SerializeField] private List<SkillNode> _skills;
        [SerializeField] private int _requiredLevel;
        public Sprite GetSkillSprite => UIDisplay;

        public abstract void ApplySkill(AliveEntity user);

        public int LevelRequirement()
        {
            return _requiredLevel;
        }
        
        public List<SkillNode> SkillRequirements()
        {
            return _skills;
        }
    }
    
    
}