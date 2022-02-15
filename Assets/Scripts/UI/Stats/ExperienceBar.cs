using Entity;
using StatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Stats
{
    public class ExperienceBar : Panel
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _experiencePercentage;
        [SerializeField] private TextMeshProUGUI _level;

        private LevelUp _levelUp;
        private AliveEntity _aliveEntity;
    
        public override void Initialize(AliveEntity aliveEntity)
        {
            _aliveEntity = aliveEntity;
            _levelUp = _aliveEntity.GetLevelData;
        
            _experiencePercentage.text = $"0%";
            _level.text = _aliveEntity.GetLevel.ToString();
        
            _levelUp.OnExperienceGivePct += LevelUpOnExperienceGivePct;
        }

        private void LevelUpOnExperienceGivePct(float pct)
        {
            string format;
        
            if (pct >= 1)
            {
                _image.fillAmount = 0;
                format = $"0%";
            }
            else
            {
                _image.fillAmount = pct;
                format = $"{pct * 100:0.##}";
            }

            _level.text = _aliveEntity.GetLevel.ToString();
            _experiencePercentage.text = $"{format}%";
        }
    }
}
