using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace SkillSystem.MainComponents.Strategies
{
    public abstract class Filtering : ScriptableObject
    {
        public abstract IEnumerable<GameObject> StartFiltering(IEnumerable<GameObject> objectToFilter,
            List<AliveEntity> userTargets);
    }
}