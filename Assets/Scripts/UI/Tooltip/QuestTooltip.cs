using QuestSystem;
using TMPro;
using UI.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Tooltip
{
    public class QuestTooltip : Tooltip
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Transform _objectiveSpawner;
        [SerializeField] private Transform _rewardSpawner;
        
        [SerializeField] private RewardDisplay _reward;
        
        [SerializeField] private GameObject _objective;
        [SerializeField] private GameObject _objectiveCompleted;

        [SerializeField] private Vector3 _offset;
        public static QuestTooltip Instance { get; private set; }

        protected override void Initialize()
        {
            Instance = this;
            HideTooltip();
        }

        public void ShowTooltip( QuestStatus status)
        {
            _title.text = status.GetQuest.GetTitle;

            foreach (var objective in status.GetQuest.GetObjectives)
            {
                var prefab = _objective;
                if (status.IsObjectiveCompleted(objective))
                {
                    prefab = _objectiveCompleted;
                }
                
                var objectivePrefab = Instantiate(prefab, _objectiveSpawner);
                objectivePrefab.GetComponentInChildren<TextMeshProUGUI>().text = objective.Description;
            }

            foreach (var reward in status.GetQuest.GetRewards)
            {
                var rewardPrefab = Instantiate(_reward, _rewardSpawner);
                rewardPrefab.SetData(reward.Item, reward.Amount);
            }

            if (status.GetQuest.GetExperience > 0)
            {
                var experiencePrefab = Instantiate(_reward, _rewardSpawner);
                experiencePrefab.SetData(status.GetQuest.GetExperience);
            }

            gameObject.SetActive(true);   
        }

        public void HideTooltip()
        {
            foreach (Transform child in _rewardSpawner)
            {
                Destroy(child.gameObject);
            }

            foreach (Transform child in _objectiveSpawner)
            {
                Destroy(child.gameObject);
            }
            gameObject.SetActive(false);
        }
    }
}