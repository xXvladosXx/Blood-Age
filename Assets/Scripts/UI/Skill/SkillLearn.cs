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
            
            _skillUpgradeData.SkillTree.OnSkillsChanged += SkillTreeOnOnSkillsChanged;
            SkillTreeOnOnSkillsChanged();
        }

        private void SkillTreeOnOnSkillsChanged()
        {
            if(_skillUpgradeData == null) return;
            
            if (_skillUpgradeData.SkillTree.HasSkill(_skill))
            {
                _foreground.gameObject.SetActive(true);
                _upgrade.gameObject.SetActive(false);
                _background.gameObject.SetActive(false);
                    
                return;
            }
            
            if (_skillUpgradeData.SkillTree.EnoughPoints())
            {
                if (!_skillUpgradeData.SkillTree.MeetsTheConditionsOfRequiredSkills(_skill) ||
                    !_skillUpgradeData.SkillTree.CheckLevel(_skill, _skillUpgradeData))
                {
                    _upgrade.gameObject.SetActive(false);
                    return;
                }
                
                _foreground.gameObject.SetActive(true);
                _upgrade.gameObject.SetActive(true);
                _background.gameObject.SetActive(false);
            }
            else
            {
                _foreground.gameObject.SetActive(false);
                _upgrade.gameObject.SetActive(false);
                _background.gameObject.SetActive(true);
            }
        }

        private void OnEnable()
        {
            _upgrade.onClick.AddListener(Unlock);
            SkillTreeOnOnSkillsChanged();
        }

        private void OnDisable()
        {
            _upgrade.onClick.RemoveListener(Unlock);
        }

        private void Unlock()
        {
            _upgrade.gameObject.SetActive(false);
            _foreground.gameObject.SetActive(true);
            _skillUpgradeData.SkillTree.UnlockSkill(_skill, _skillUpgradeData);
        }
        
    }
}