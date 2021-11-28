namespace DefaultNamespace.UI.ButtonClickable
{
    using System;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using UnityEngine;

    public abstract class EffectApplying : ScriptableObject
    {
        public abstract void Effect(SkillData skillData, Action finished);
    }
}