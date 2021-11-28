namespace DefaultNamespace.SkillSystem.SkillNodes
{
    using DefaultNamespace.SkillSystem.SkillInfo;
    using UnityEngine;

    public abstract class SkillNode : ScriptableObject
    {
        public abstract void ApplySkill(GameObject user);
    }
}