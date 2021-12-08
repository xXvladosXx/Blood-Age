using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Entity;
using DefaultNamespace.MouseSystem;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private AttackData _attackData;
    private GameObject _hitParticlePrefab;
    
    private List<HitEffectInstantiator> _hitEnemy = new List<HitEffectInstantiator>();
    private List<AliveEntity> _aliveEntities = new List<AliveEntity>();

    private float _damage;
    private float _maxDamage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HitEffectInstantiator hitEffectInstantiator))
        {
            hitEffectInstantiator.GenerateParticleEffectHit(_hitParticlePrefab, transform);
            _hitEnemy.Add(hitEffectInstantiator);
        }
        
        if (other.TryGetComponent(out AliveEntity aliveEntity) && other.GetComponent<AliveEntity>() != transform.GetComponentInParent<AliveEntity>())
        {
            _damage = _attackData.Damage;
            _aliveEntities.Add(aliveEntity);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<AliveEntity>() == null) return;
        if(other.GetComponent<AliveEntity>() == transform.GetComponentInParent<AliveEntity>()) return;
    
        _damage += Time.deltaTime;
        _damage = Mathf.Clamp(_damage, 0, _attackData.MaxDamage);
        _attackData.Damage = _damage;
        
        other.GetComponent<AliveEntity>().GetHealth.TakeHit(_attackData);
    }
    private void OnTriggerExit(Collider other)
    {
        _damage = 0;
        _attackData.Damage = _damage;

        if (other.TryGetComponent(out HitEffectInstantiator hitEffectInstantiator))
        {
            _hitEnemy.Remove(hitEffectInstantiator);
            hitEffectInstantiator.DestroyParticleHit();
        }
        
        if (other.TryGetComponent(out AliveEntity aliveEntity))
        {
            _damage = _attackData.Damage;
            _aliveEntities.Remove(aliveEntity);
        }
    }

    public void ReceiveData(AttackData attackData, GameObject particleEffect)
    {
        _attackData = attackData;
        _hitParticlePrefab = particleEffect;
    }

    private void OnDestroy()
    {
        foreach (var other in _hitEnemy)
        {
            other.DestroyParticleHit();
        }
    }
}
