using Entity;
using Sirenix.OdinInspector;
using SkillSystem.SkillNodes;
using StatsSystem;
using UnityEngine;

namespace SkillSystem.Skills
{
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