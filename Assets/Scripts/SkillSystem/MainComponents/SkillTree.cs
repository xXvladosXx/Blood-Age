namespace DefaultNamespace.UI.ButtonClickable
{
    using System;
    using System.Collections.Generic;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillTree : MonoBehaviour
    {
        private List<SkillNode> _unlockedSkills;

        private void Awake()
        {
            _unlockedSkills = new List<SkillNode>();
            
            foreach (var skillHolder in GetComponentsInChildren<SkillHolder>())
            {
                if (skillHolder.Active)
                {
                    _unlockedSkills.Add(skillHolder.GetMainSkill);
                }
            }

            foreach (var skill in _unlockedSkills)
            {
                print(skill);
            }
        }

        public void UpdateSkills(SkillHolder skillHolder)
        {
            if (!_unlockedSkills.Contains(skillHolder.GetMainSkill))
            {
                _unlockedSkills.Add(skillHolder.GetMainSkill);
            }
            
            foreach (var skill in _unlockedSkills)
            {
                print(skill);
            }
        }

        public bool SatisfiesRequirements(SkillHolder skillHolder)
        {
            return skillHolder.GetSkillRequire == null || _unlockedSkills.Contains(skillHolder.GetSkillRequire);
        }
    }
}