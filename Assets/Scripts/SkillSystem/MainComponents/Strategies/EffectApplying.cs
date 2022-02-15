using System;
using SkillSystem.SkillInfo;
using UnityEngine;

namespace SkillSystem.MainComponents.Strategies
{
    public abstract class EffectApplying : ScriptableObject
    {
        public abstract void Effect(SkillData skillData, Action cancel, Action finished);
    }
}