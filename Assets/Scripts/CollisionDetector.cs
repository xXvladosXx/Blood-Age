using System;
using System.Collections;
using System.Collections.Generic;
using AttackSystem.AttackMechanics;
using DefaultNamespace;
using Entity;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private AttackData _attackData;
    private GameObject _hitParticlePrefab;
    
    private List<AliveEntity> _aliveEntities = new List<AliveEntity>();

    private float _damage;
    private float _maxDamage;
    private float _delayToDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AliveEntity aliveEntity) && aliveEntity != _attackData.Damager)
        {
            _damage = _attackData.Damage;
            _aliveEntities.Add(aliveEntity);

            StartCoroutine(StartDamage());
        }
    }

    private IEnumerator StartDamage()
    {
        while (true)
        {
            foreach (var aliveEntity in _aliveEntities)
            {
                aliveEntity.GetHealth.TakeHit(_attackData);
            }

            yield return new WaitForSeconds(_delayToDamage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // if(!other.TryGetComponent(out AliveEntity aliveEntity)) return;
        // if(aliveEntity == _attackData.Damager) return;
        //
        // _damage += Time.deltaTime;
        // _damage = Mathf.Clamp(_damage, 0, _attackData.MaxDamage);
        // _attackData.Damage = _damage;
        //
        // aliveEntity.GetHealth.TakeHit(_attackData);
       
    }
    private void OnTriggerExit(Collider other)
    {
        _damage = 0;
        _attackData.Damage = _damage;

        if (other.TryGetComponent(out AliveEntity aliveEntity))
        {
            _damage = _attackData.Damage;
            _aliveEntities.Remove(aliveEntity);
        }
    }

    private void OnDestroy()
    {
        _aliveEntities.Clear();
    }

    public void SendData(AttackData attackData, GameObject particleEffect, float delayToDamage)
    {
        _attackData = attackData;
        _hitParticlePrefab = particleEffect;
        _delayToDamage = delayToDamage;
    }
}
