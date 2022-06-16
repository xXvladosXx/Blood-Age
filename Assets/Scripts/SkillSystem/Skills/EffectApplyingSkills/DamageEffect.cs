using System;
using System.Collections;
using System.Collections.Generic;
using AttackSystem.AttackMechanics;
using Entity;
using SkillSystem.SkillInfo;
using Unity.Mathematics;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private AliveEntity _user;
    [SerializeField] private float _damage;
    [SerializeField] private GameObject[] _particels;
    [SerializeField] private bool _terrainCollision;
    private AttackData _attackData;

    public void SetData(SkillData skillData, AttackData attackData)
    {
        _user = skillData.GetUser;
        _attackData = attackData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is TerrainCollider terrainCollider)
        {
            foreach (var particle in _particels)
            {
                var instParticle = Instantiate(particle, transform);
            }
            
            var hits = Physics.SphereCastAll(transform.position, _radius, Vector3.up, 100);

            foreach (var raycastHit in hits)
            {
                raycastHit.transform.TryGetComponent(out AliveEntity target);
                if(target == _user) continue;
                if(target == null) continue;
                if(!_attackData.Targets.Contains(target)) continue;

                target.GetHealth.TakeHit(_attackData);
            }
        }
        
        if(_terrainCollision) return;

        other.TryGetComponent(out AliveEntity target1);
        if(target1 == _user) return;
        if(target1 == null) return;
        if(!_attackData.Targets.Contains(target1)) return;

        target1.GetHealth.TakeHit(_attackData);
    }
}