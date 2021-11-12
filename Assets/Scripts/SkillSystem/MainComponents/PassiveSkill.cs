namespace SkillSystem.MainComponents
{
    using DefaultNamespace;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using Sirenix.OdinInspector;
    using UnityEngine;
    
    [ShowOdinSerializedPropertiesInInspector]
    [CreateAssetMenu (menuName = "Skills/Passive/HealthBonus")]
    public class PassiveSkill : SkillNode
    {
        public PlayerPassiveSkillBonus[] _playerPassiveSkillBonus;
        
        
        public override void ApplySkill(FindStats findStats)
        {
           
        }

    }
}