using System.Globalization;
using TMPro;
using UnityEngine;

namespace UI.Skill
{
    public class SkillInfoDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _skillText;
        [SerializeField] private TextMeshProUGUI _skillValue;
        
        public void SetData(string skillBonus, float skillValue)
        {
            _skillText.text = skillBonus;
            _skillValue.text = skillValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}