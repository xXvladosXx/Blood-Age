using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.AIDialogue;
using Entity;
using QuestSystem;
using SaveSystem;
using SceneSystem;
using StateMachine.PlayerStates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EntitySpawnerSystem
{
    public class EntitySpawner : MonoBehaviour, ISavable
    {
        public static EntitySpawner Instance { get; private set; }
    
        [SerializeField] private EnemyWave[] _enemyWaves;
        [SerializeField] private AIConversant[] _aiConversants;
        [SerializeField] private Portal _portalToActivate;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private GameObject _spawnParticel;
        [SerializeField] private float _spawnParticleDestroy;
        [SerializeField] private float _timeBetweenWaves = 5f;
    
        private List<AliveEntity> _enemies = new List<AliveEntity>();
        private List<AliveEntity> _deadEnemies = new List<AliveEntity>();
        private List<AliveEntity> _allEntities = new List<AliveEntity>();

        private EnemyWave _currentEnemyWave;
        private PlayerEntity _playerEntity;
        private BoxCollider _boxCollider;

        private int _waveTime;
        private int _waveIndex;
        private bool _isPortalActive;
        private PlayerQuestList _playerQuestList;
        public float GetTimeBetweenWaves => _timeBetweenWaves;
        public List<AliveEntity> GetAllAliveEntities => _allEntities;
        public Portal GetPortal => _portalToActivate;

        public event Action<int> OnWaveStartSpawn;
    
        private void Awake()
        {
            Instance = this;
            _playerEntity = PlayerStateManager.GetPlayer;
            _playerQuestList = _playerEntity.GetComponent<PlayerQuestList>();

            _currentEnemyWave = _enemyWaves.FirstOrDefault();
            _waveIndex = 0;
            _boxCollider = GetComponent<BoxCollider>();

            if(_aiConversants == null) return;
            foreach (var aiConversant in _aiConversants)
            {
                aiConversant.gameObject.SetActive(true);
                _allEntities.Add(aiConversant);
            }
        }

        private void OnEnable()
        {
            _playerQuestList.OnPortalRequest += ActivatePortal;
        }

        private void OnDisable()
        {
            _playerQuestList.OnPortalRequest -= ActivatePortal;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerEntity playerEntity))
            {
                StartCoroutine(SpawnEnemies());
                _boxCollider.enabled = false;
            }
        }

        private IEnumerator SpawnEnemies()
        {
            _waveTime = (int) _timeBetweenWaves;
            while (_waveTime > 0)
            {
                OnWaveStartSpawn?.Invoke(_waveTime);
                _waveTime--;
                yield return new WaitForSeconds(1);
            }
        
            foreach (var aliveEntity in _currentEnemyWave.Enemy)
            {
                AddToEnemyList(aliveEntity.GetComponent<AliveEntity>());
                _allEntities.Add(aliveEntity.GetComponent<AliveEntity>());
                int randomSpawnPoint = Random.Range(0, _spawnPoints.Length);
                aliveEntity.transform.position = _spawnPoints[randomSpawnPoint].position;
                var spawnEffect = Instantiate(_spawnParticel, aliveEntity.transform.position, Quaternion.identity);
                aliveEntity.gameObject.SetActive(true);

                Destroy(spawnEffect, _spawnParticleDestroy);
                yield return new WaitForSeconds(1f);
            }
        
            yield break;
        }

        public void ActivatePortal(bool activate)
        {
            _portalToActivate.gameObject.SetActive(activate);
            _isPortalActive = activate;
        }

        private void AddToEnemyList(AliveEntity entity)
        {
            _enemies.Add(entity);
        
            entity.OnDied += EntityOnDied;
        
            entity.Targets.Add(_playerEntity);
            PlayerStateManager.GetPlayer.Targets = _enemies;
        }

        private void EntityOnDied(AliveEntity entity)
        {
            _enemies.Remove(entity);
            _deadEnemies.Add(entity);
            _playerEntity.Targets.Remove(entity);
        
            if(_enemies.Count == 0)
            {
                _waveIndex++;
                if (_enemyWaves.Length <= _waveIndex)
                {
                    ActivatePortal(true);
                    return;
                }
            
                _currentEnemyWave = _enemyWaves[_waveIndex];
                StartCoroutine(SpawnEnemies());
            }
        }

        public object CaptureState()
        {
            var saver = new EntitySpawnerSaver
            {
                EnemyCount = _enemies.Count,
                IsPortalAtive = _isPortalActive
            };
        
            return saver;
        }

        public void RestoreState(object state)
        {
            var saver = (EntitySpawnerSaver) state;
            foreach (var enemy in _enemies)
            {
                enemy.ReloadEntity();
                enemy.gameObject.SetActive(false);
            }
            foreach (var enemy in _deadEnemies)
            {
                enemy.ReloadEntity();
                enemy.gameObject.SetActive(false);
            }
        
            _currentEnemyWave = _enemyWaves.FirstOrDefault();
        
            _waveIndex = 0;
            if(_boxCollider != null)
                _boxCollider.enabled = true;
        
            _isPortalActive = saver.IsPortalAtive;

            ActivatePortal(_isPortalActive);
        }
    
        [Serializable]
        public class EntitySpawnerSaver
        {
            public bool IsPortalAtive;
            public int EnemyCount;
        }
    }
}