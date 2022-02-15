/*
using System.Collections.Generic;
using SkillSystem.MainComponents;
using SkillSystem.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Skill
{
    public class SkillDisplay : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _foreground;

        private ActiveSkill _skill;
        private Dictionary<ActiveSkill, float> _skills;
        private Sprite _defaultSprite;

        private void Awake()
        {
            _defaultSprite = _background.sprite;
        }

        private void Update()
        {
            if(_skill == null) return;
            if(_skills == null) return;
            if(_skills.Count == 0) return;
            if (!_skills.ContainsKey(_skill)) return;
        
        
            _foreground.fillAmount = _skills[_skill] / _skill.GetCooldown;

            if (_skills[_skill] < 0)
            {
                _foreground.fillAmount = 1;
            }
        }

        public void SetSkill(ActiveSkill activeSkill)
        {
            _skill = activeSkill;
        
            _foreground.fillAmount = 1;
            _background.sprite = activeSkill.GetSkillSprite;
            _foreground.sprite = activeSkill.GetSkillSprite;
        }

        public void ResetSkill()
        {
            _skill = null;
            _foreground.sprite = _defaultSprite;
            _background.sprite = _defaultSprite;
        }

        public void SetSkills(Dictionary<ActiveSkill, float> skills)
        {
            _skills = skills;
        }
    }
}
*/
