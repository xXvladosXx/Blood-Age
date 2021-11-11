namespace DefaultNamespace.UI
{
    using System;
    using System.Linq;
    using DefaultNamespace.SkillSystem;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using UnityEngine;

    [Serializable]
    public class SkillChoice
    {
        public string Choice;
        public SkillNode SkillNode;
    }

    // [CreateAssetMenu(menuName = "Skill/Node/Choice")]
    // public class SkillNodeChoice : SkillNode
    // {
    //     [SerializeField] private SkillChoice[] _choices;
    //     public SkillChoice[] GetChoices => _choices;
    //     
    //    
    // }
}