using System;
using SkillSystem;
using SkillSystem.SkillNodes;
using SkillSystem.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Skill
{
    public class SkillLearn : MonoBehaviour
    {
        [SerializeField] private Button _upgrade;
        [SerializeField] private Image _background;
        [SerializeField] private Image _foreground;

        private SkillNode _skill;
        private SkillUpgradeData _skillUpgradeData;

        private void Awake()
        {
            _foreground.gameObject.SetActive(false);
        }

        public void SetSkill(SkillNode activeSkill, SkillUpgradeData skillUpgradeData)
        {
            _skill = activeSkill;
            _skillUpgradeData = skillUpgradeData;
            _background.sprite = activeSkill.GetSkillSprite;
            _foreground.sprite = activeSkill.GetSkillSprite;
            
            if (_skillUpgradeData.SkillTree._knownSkillsList.Contains(_skill))
            {
                _upgrade.gameObject.SetActive(false);
                _foreground.gameObject.SetActive(true);
                _skillUpgradeData.SkillTree.UnlockSkill(_skill, _skillUpgradeData);
                return;
            }
            
            _skillUpgradeData.SkillTree.OnSkillsChanged += SkillTreeOnOnSkillsChanged;
            
            var canAssign = _skillUpgradeData.SkillTree.CheckDependencies(_skill, _skillUpgradeData);
            _upgrade.gameObject.SetActive(canAssign);
        }

        private void SkillTreeOnOnSkillsChanged()
        {
            var canAssign = _skillUpgradeData.SkillTree.CheckDependencies(_skill, _skillUpgradeData);
            _upgrade.gameObject.SetActive(canAssign);
        }

        private void OnEnable()
        {
            _upgrade.onClick.AddListener(Unlock);
        }

        private void OnDisable()
        {
            _upgrade.onClick.RemoveListener(Unlock);
        }

        private void Unlock()
        {
            _skillUpgradeData.Points--;
            _upgrade.gameObject.SetActive(false);
            _foreground.gameObject.SetActive(true);
            _skillUpgradeData.SkillTree.UnlockSkill(_skill, _skillUpgradeData);
        }
        
    }
}