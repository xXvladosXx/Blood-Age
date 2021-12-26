namespace DefaultNamespace.SkillSystem.SkillNodes
{
    using DefaultNamespace.Entity;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using UnityEngine;

    public abstract class SkillNode : ScriptableObject
    {
        public abstract void ApplySkill(AliveEntity user);
    }
}