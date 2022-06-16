using System;
using System.Collections.Generic;
using DialogueSystem;
using DialogueSystem.AIDialogue;
using Entity;
using EntitySpawnerSystem;
using QuestSystem.QuestObjectives;
using SceneSystem;
using StatsSystem;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }
        
        [SerializeField] private Transform _pointer;
        [SerializeField] private PlayerConversant _playerConversant;

        private PlayerQuestList _playerQuestList;
        private Dictionary<Class, GameObject> _entities;
        private Transform _target;
        
        private void Awake()
        {
            Instance = this;
            _playerQuestList = _playerConversant.GetComponent<PlayerQuestList>();
        }

        private void Start()
        {
            _playerQuestList.OnEntityRequest += FindNecessaryEntity;
            _playerQuestList.OnPortalRequest += FindNecessaryPortal;
            _playerQuestList.OnItemRequest += RemoveTarget;
            _playerQuestList.OnNullRequest += RemoveTarget;
        }

        private void RemoveTarget()
        {
            _target = null;
        }

        private void FindNecessaryPortal(bool activate)
        {
            if(activate)
                _target = EntitySpawner.Instance.GetPortal.transform;
        }

        private void FindNecessaryEntity(Class obj)
        {
            var aliveEntities = EntitySpawner.Instance.GetAllAliveEntities;
            foreach (var aliveEntity in aliveEntities)
            {
                if(aliveEntity == null) continue;
                if (aliveEntity.SerializableClass == obj)
                {
                    _target = aliveEntity.transform;
                    break;
                }
            }
        }

        private void Update()
        {
            if (_target != null)
            {
                _pointer.transform.LookAt(_target);
                _pointer.gameObject.SetActive(true);
            }
            else
            {
                _pointer.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _playerQuestList.OnEntityRequest -= FindNecessaryEntity;
            _playerQuestList.OnPortalRequest -= FindNecessaryPortal;
            _playerQuestList.OnItemRequest -= RemoveTarget;
            _playerQuestList.OnNullRequest -= RemoveTarget;
        }
    }
}