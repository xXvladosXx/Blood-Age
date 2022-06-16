using System;
using Entity;
using SkillSystem;
using TMPro;
using UnityEngine;

namespace UI.Skill
{
    public class SkillPanel : Panel
    {
        [SerializeField] private TextMeshProUGUI _skillPoints;
        
        private AliveEntity _aliveEntity;
        private SkillLearn[] _skillUpgrades;
        private SkillTree _skillTree;
        private SkillUpgradeData _skillUpgradeData;

        public override void Initialize(AliveEntity aliveEntity)
        {
            _skillTree = aliveEntity.GetComponent<SkillTree>();
            _aliveEntity = aliveEntity;
            _skillPoints.text = _skillTree.GetPoints.ToString();
            _skillTree.OnSkillsChanged += FindPoints;

            SetSkillsData();
        }

        private void FindPoints()
        {
            _skillPoints.text = _skillTree.GetPoints.ToString();
        }

        private void SetSkillsData()
        {
            _skillUpgradeData = new SkillUpgradeData(_skillTree, _aliveEntity);
            _skillUpgrades = GetComponentsInChildren<SkillLearn>();

            for (int i = 0; i < _skillTree._unknownSkillsList.Count; i++)
            {
                _skillUpgrades[i].SetSkill(_skillTree._unknownSkillsList[i], _skillUpgradeData);
            }
        }
    }
    
    [Serializable]
    public class SkillUpgradeData
    {
        public SkillTree SkillTree;
        public AliveEntity Entity;

        public SkillUpgradeData(SkillTree skillTree, AliveEntity entity)
        {
            Entity = entity;
            SkillTree = skillTree;
        }

        public void RemovePoints()
        {
            SkillTree.UpgradeSkill();
        }
    }
}