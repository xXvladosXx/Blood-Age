using System;
using System.Collections.Generic;
using Entity;
using QuestSystem;
using UnityEngine;

namespace UI.Quests
{
    public class QuestListPanel : Panel
    {
        [SerializeField] private QuestDisplay _questDisplay;
        [SerializeField] private Transform _content;

        private List<QuestDisplay> _questDisplays = new List<QuestDisplay>();
        private AliveEntity _playerEntity;
        private PlayerQuestList _playerQuestList;

        public override void Initialize(AliveEntity aliveEntity)
        {
            _playerEntity = aliveEntity;
            _playerQuestList = _playerEntity.GetComponent<PlayerQuestList>();
            UpdateQuestList();

            _playerQuestList.OnQuestUpdate += UpdateQuestList;
        }

        private void OnEnable()
        {
            if (_playerQuestList == null) return;
            UpdateQuestList();
        }

        private void UpdateQuestList()
        {
            _questDisplays.Clear();
            foreach (Transform child in _content) {
                Destroy(child.gameObject);
            }

            foreach (var questStatus in _playerQuestList.GetStatuses)
            {
                var questPrefab = Instantiate(_questDisplay, _content);
                questPrefab.SetQuest(questStatus);
                _questDisplays.Add(questPrefab);
            }
        }
    }
}