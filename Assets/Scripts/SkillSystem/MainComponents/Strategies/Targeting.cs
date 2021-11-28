namespace DefaultNamespace.UI.ButtonClickable
{
    using System;
    using DefaultNamespace.SkillSystem.SkillInfo;
    using UnityEngine;

    public abstract class Targeting : ScriptableObject
    {
        public abstract void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack);
    }
}