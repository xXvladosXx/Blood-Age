using Entity;
using UI;
using UI.Skill;

namespace SkillSystem
{
    using System;
    using UnityEngine;

    public class SkillPanel : Panel
    {
        [SerializeField] private int _points;
        
        private SkillLearn[] _skillUpgrades;
        
        private SkillTree _skillTree;
        private SkillUpgradeData _skillUpgradeData;
        private int Points;

        public override void Initialize(AliveEntity aliveEntity)
        {
            _skillTree = aliveEntity.GetComponent<SkillTree>();
            
            _skillUpgradeData = new SkillUpgradeData(Points, _skillTree, aliveEntity);

            _skillUpgrades = GetComponentsInChildren<SkillLearn>();
            
            for (int i = 0; i < _skillTree._unknownSkillsList.Count; i++)
            {
                _skillUpgrades[i].SetSkill(_skillTree._unknownSkillsList[i], _skillUpgradeData);
            }
        }

        private void Update()
        {
            
        }
    }
    
    [Serializable]
    public class SkillUpgradeData
    {
        public int Points;
        public SkillTree SkillTree;
        public AliveEntity Entity;

        public SkillUpgradeData(int points, SkillTree skillTree, AliveEntity entity)
        {
            Points = points;
            Entity = entity;
            SkillTree = skillTree;
        }
    }
}