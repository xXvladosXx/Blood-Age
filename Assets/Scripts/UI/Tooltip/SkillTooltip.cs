using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using InventorySystem;
using SkillSystem.Skills;
using TMPro;
using UI.Skill;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Tooltip
{
    public class SkillTooltip : DynamicTooltip
    {
        public static SkillTooltip Instance { get; private set; }
        
        [SerializeField] private TextMeshProUGUI _skillName;
        [SerializeField] private TextMeshProUGUI _cooldown;
        [SerializeField] private TextMeshProUGUI _type;
        [SerializeField] private TextMeshProUGUI _bonus;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _requirements;
        [SerializeField] private Image _clock;

        protected override void Initialize()
        {
            Instance = this;
        
            HideTooltip();
        }
        
        public void ShowTooltip(Item overlappedInventoryItem)
        {
            if(overlappedInventoryItem == null) return;
            StringBuilder topLevelString = new StringBuilder();
            
            if (overlappedInventoryItem is ActiveSkill activeSkill)
            {
                _type.text = activeSkill.Description;
                _skillName.text = activeSkill.Data.Name;
                _requirements.text = "";
                _bonus.text = "";
                _clock.enabled = true;

                foreach (var data in activeSkill.GetData())
                {
                    if (data.Key == "Cooldown")
                    {
                        _cooldown.text = data.Value.ToString();
                        continue;
                    }

                    if (data.Key == "Bonus")
                    {
                        _bonus.text = data.Value.ToString();
                        continue;
                    }

                    if (data.Key == "Requirements")
                    {
                        _requirements.text = data.Value.ToString();
                        continue;
                    }
                    
                    topLevelString.Append(data.Value);
                }

                _text.text = topLevelString.ToString();
                Update();
                gameObject.SetActive(true);
            }else if (overlappedInventoryItem is ActiveSkillUpgrade activeSkillUpgrade)
            {
                _skillName.text = activeSkillUpgrade.Data.Name;
                _type.text = activeSkillUpgrade.Description;
                _bonus.text = "";
                _text.text = "";
                _cooldown.text = "";
                _requirements.text = "";
                _clock.enabled = false;

                Update();
                gameObject.SetActive(true);
            }
        }
        
        
        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }
    }
}