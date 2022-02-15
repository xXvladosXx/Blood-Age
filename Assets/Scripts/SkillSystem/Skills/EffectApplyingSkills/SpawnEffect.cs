using System;
using System.Collections;
using System.Collections.Generic;
using AttackSystem.AttackMechanics;
using DefaultNamespace.SkillSystem.SkillInfo;
using Runemark.Common;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SkillSystem.Skills.EffectApplyingSkills
{
    [CreateAssetMenu(menuName = "Skill/Effect/SpawnEffect")]
    public class SpawnEffect : EffectApplying, ICollectable
    {
        [SerializeField] private GameObject _gameObjectToInstanciate;
        [SerializeField] private GameObject[] _gameObjectsToInstanciate;
        [SerializeField] private float _delay;
        [SerializeField] private float _timeToDestroy;
        [SerializeField] private bool _mousePosition;
        [SerializeField] private bool _playerPosition;
        [SerializeField] private bool _hasOwnDamageScript;
        [SerializeField] private bool _randomOffset;
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistace;
        [SerializeField] private float _damage;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _criticalChance;
        [SerializeField] private float _criticalDamage;
        
        private GameObject _instantiatedGameObject;
        private float _xOffset;
        private float _zOffset;
       

        public override void Effect(SkillData skillData, Action cancel, Action finished)
        {
            if (_randomOffset)
            {
                _xOffset = Random.Range(_minDistance, _maxDistace);
                _zOffset = Random.Range(_minDistance, _maxDistace);
            }

            skillData.StartCoroutine(StartInstantiating(skillData, finished));
        }

        private IEnumerator StartInstantiating(SkillData skillData, Action finished)
        {
            yield return new WaitForSeconds(_delay);
            Quaternion playerRotation = skillData.GetUser.transform.rotation;
            Vector3 vector3;

            if (_mousePosition)
            {
                vector3 = skillData.MousePosition;
                _instantiatedGameObject =
                    Instantiate(_gameObjectToInstanciate, vector3, playerRotation);
            }
            else if (_playerPosition)
            {
                vector3 = skillData.GetUser.transform.position;
                _instantiatedGameObject =
                    Instantiate(_gameObjectToInstanciate,vector3 , playerRotation);
            }
            else
            {
                vector3 = skillData.MousePosition + _offset + new Vector3(_xOffset, 0, _zOffset);
                playerRotation = Quaternion.identity;
                
                _instantiatedGameObject =
                    Instantiate(_gameObjectToInstanciate, vector3, playerRotation);
            }

            if (_instantiatedGameObject.TryGetComponent(out DamageEffect damageEffect))
            {
                damageEffect.SetData(skillData, new AttackData
                {
                    Damage = _damage,
                    Damager = skillData.GetUser,
                    Targets = skillData.GetUser.Targets,
                    Accuracy = 100,
                    CriticalChance = _criticalChance,
                    CriticalDamage = _criticalDamage
                });
            }
            InstantiateGameObjects(skillData, vector3, playerRotation);

            Destroy(_instantiatedGameObject, _timeToDestroy);
            finished();
        }

        private void InstantiateGameObjects(SkillData skillData, Vector3 position, Quaternion rotation)
        {
            foreach (var gameObject in _gameObjectsToInstanciate)
            {
                var o = Instantiate(gameObject, position, rotation);
                if (o.TryGetComponent(out DamageEffect damageEffect))
                {
                     damageEffect = o.GetComponent<DamageEffect>();
                     damageEffect.SetData(skillData, new AttackData
                     {
                         Damage = _damage,
                         Damager = skillData.GetUser,
                         Targets = skillData.GetUser.Targets,
                         Accuracy = 100,
                         CriticalChance = _criticalChance,
                         CriticalDamage = _criticalDamage
                     });
                }
                Destroy(o, _timeToDestroy);
            }   
        }

        public void AddData(Dictionary<string, float> data)
        {
            if(_damage != 0)
                data.Add("Damage", _damage);
        }
    }
}