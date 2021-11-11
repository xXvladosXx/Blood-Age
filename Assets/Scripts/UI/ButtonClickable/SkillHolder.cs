namespace DefaultNamespace.UI.ButtonClickable
{
    using System;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using UnityEngine;

    public class SkillHolder : MonoBehaviour
    {
        [SerializeField] private SkillNode _mainSkillNode;
        public SkillNode GetMainSkill => _mainSkillNode;
        
        [SerializeField] private SkillNode _skillNodeRequire;
        public SkillNode GetSkillRequire => _skillNodeRequire;
        
        public bool Active { get; set; }

        private void Update()
        {
            
        }
    }
}