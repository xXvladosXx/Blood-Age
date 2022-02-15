using System.Collections.Generic;
using System.Linq;
using Entity;
using SkillSystem.MainComponents.Strategies;
using UnityEngine;

namespace SkillSystem.Skills.FilteringSkills
{
    [CreateAssetMenu(menuName = "Skill/Filtering/HealFilter")]
    public class HealFilter : Filtering
    {
        public override IEnumerable<GameObject> StartFiltering(IEnumerable<GameObject> objectToFilter,
            List<AliveEntity> userTargets)
        {
            if (objectToFilter == null) return null;

            return objectToFilter.Where(gameObject => gameObject.TryGetComponent(out IHealable healable)
                                                      && !userTargets.Contains(healable));
        }
    }
}