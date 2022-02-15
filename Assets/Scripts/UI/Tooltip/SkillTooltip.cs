using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using InventorySystem;
using SkillSystem.Skills;
using TMPro;
using UI.Skill;
using UnityEngine;

namespace UI.Tooltip
{
    public class SkillTooltip : Tooltip
    {
        public static SkillTooltip Instance { get; private set; }
        
        [SerializeField] private TextMeshProUGUI _skillName;
        [SerializeField] private SkillInfoDisplay _skillInfoDisplay;
        [SerializeField] private TextMeshProUGUI _cooldown;
        [SerializeField] private TextMeshProUGUI _type;
        [SerializeField] private TextMeshProUGUI _area;

        private StringBuilder _stringBuilder;
        private List<SkillInfoDisplay> _skillInfoDisplays = new List<SkillInfoDisplay>();
        protected override void Initialize()
        {
            Instance = this;
        
            HideTooltip();
        
            _stringBuilder = new StringBuilder();
        }
        
        public void ShowTooltip(Item overlappedInventoryItem)
        {
            if(overlappedInventoryItem == null) return;
            
            if (overlappedInventoryItem is ActiveSkill activeSkill)
            {
                _skillName.text = activeSkill.Data.Name;

                var sortedDictionary = activeSkill.GetData().OrderBy(x => x.Key).
                    ToDictionary(x => x.Key, y => y.Value);
                
                foreach (var data in sortedDictionary)
                {
                    if (data.Key == "Cooldown")
                    {
                        _cooldown.text = data.Value.ToString(CultureInfo.InvariantCulture);
                        continue;
                    }
                    
                    var skillInfo = Instantiate(_skillInfoDisplay, transform);
                    _skillInfoDisplays.Add(skillInfo);
                    skillInfo.SetData(data.Key, data.Value);
                }

                gameObject.SetActive(true);
            }
        }
        
        
        public void HideTooltip()
        {
            gameObject.SetActive(false);

            foreach (var skillInfoDisplay in _skillInfoDisplays)
            {
                Destroy(skillInfoDisplay.gameObject);
            }
            
            _skillInfoDisplays.Clear();
        }
    }
}