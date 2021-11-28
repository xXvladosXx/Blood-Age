namespace SkillSystem.MainComponents.FilteringSkills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace.UI.ButtonClickable;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Skill/Filtering/FilterByTag")]
    public class TagFilter : Filtering
    {
        [SerializeField] private String _tag;
        
        public override IEnumerable<GameObject> StartFiltering(IEnumerable<GameObject> objectToFilter)
        {
            return objectToFilter.Where(gameObject => gameObject.CompareTag(_tag));
        }
    }
}