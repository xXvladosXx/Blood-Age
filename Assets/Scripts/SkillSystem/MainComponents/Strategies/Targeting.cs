using System;
using SkillSystem.SkillInfo;
using UnityEngine;

namespace SkillSystem.MainComponents.Strategies
{
    public abstract class Targeting : ScriptableObject
    {
        public abstract void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack);
    }
}