using System;
using QuestSystem;
using TMPro;
using UI.Tooltip;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Quests
{
    public class QuestDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _completedQuests;
        [SerializeField] private TextMeshProUGUI _questTitle;

        private QuestStatus _questStatus;
        
        public void SetQuest(QuestStatus status)
        {
            _questStatus = status;
            _completedQuests.text = $"{status.GetCompletedObjectivesCount}/{status.GetQuest.GetProgress}";
            _questTitle.text = status.GetQuest.GetTitle;
        }

        private void OnDisable()
        {
            QuestTooltip.Instance.HideTooltip();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            QuestTooltip.Instance.ShowTooltip( _questStatus);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            QuestTooltip.Instance.HideTooltip();
        }
    }
}