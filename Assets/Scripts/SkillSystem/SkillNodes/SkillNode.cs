using System.Collections.Generic;
using Entity;
using InventorySystem;
using UnityEngine;

namespace SkillSystem.SkillNodes
{
    public abstract class SkillNode : Item
    {
        [SerializeField] protected List<SkillNode> Skills;
        [SerializeField] protected int RequiredLevel;
        public Sprite GetSkillSprite => UIDisplay;

        public abstract void ApplySkill(AliveEntity user);

        public int LevelRequirement()
        {
            return RequiredLevel;
        }
        
        public List<SkillNode> SkillRequirements()
        {
            return Skills;
        }
    }
    
    
}