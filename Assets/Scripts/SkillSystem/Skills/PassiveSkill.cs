namespace SkillSystem.MainComponents
{
    using DefaultNamespace;
    using DefaultNamespace.Entity;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using Sirenix.OdinInspector;
    using UnityEngine;
    
    [ShowOdinSerializedPropertiesInInspector]
    [CreateAssetMenu (menuName = "Skill/Passive/HealthBonus")]
    public class PassiveSkill : SkillNode
    {
        public CharacteristicBonus[] _playerPassiveSkillBonus;
        
        public override void ApplySkill(AliveEntity user)
        {
           
        }

    }
}