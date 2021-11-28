using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.MouseSystem;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private AttackData _attackData;
    private GameObject _hitParticlePrefab;
    private List<HitEffectInstantiator> _hitEnemy = new List<HitEffectInstantiator>();

    private float _damage;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HitEffectInstantiator hitEffectInstantiator))
        {
            hitEffectInstantiator.GenerateParticleEffectHit(_hitParticlePrefab, transform);
            _hitEnemy.Add(hitEffectInstantiator);
        }
        
        if (other.GetComponent<Health>() != null)
        {
            _damage = _attackData.Damage;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Health>() == null) return;
        if(other.GetComponent<Health>() == transform.GetComponentInParent<Health>()) return;

        _damage += Time.deltaTime;
        _attackData.Damage = _damage;
        
        other.GetComponent<Health>().TakeHit(_attackData);
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
