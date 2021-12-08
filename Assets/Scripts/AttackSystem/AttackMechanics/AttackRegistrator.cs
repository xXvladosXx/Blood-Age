using System;
using System.Collections;
using System.Collections.Generic;
using AttackSystem;
using Cinemachine;
using DefaultNamespace;
using DefaultNamespace.Entity;
using DefaultNamespace.MouseSystem;
using InventorySystem;
using UnityEngine;
using UnityEngine.VFX;

public class AttackRegistrator : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private Transform _arrowSpawnPosition;

    [SerializeField] private float _arrowSpeed = 15f;
    [SerializeField] private float _stickSpeed = 15f;

    public AttackData AttackData;
    private SphereCollider _sphereCollider;
  
    private AliveEntity _aliveEntity;

    public event Action<Transform> OnHitTake;


    private void Awake()
    {
        AttackData = new AttackData();

        _sphereCollider = GetComponent<SphereCollider>();
        _aliveEntity = GetComponentInParent<AliveEntity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AliveEntity>() == null) return;
        if (other.GetComponent<AliveEntity>() == _aliveEntity) return;

        // if (other.GetComponent<StarterAssetsInputs>() != null && AttackData.HeavyAttack)
        // {
        //     _cinemachineVirtualCamera.enabled = true;
        // }

        print("Damaged");
        FindNecessaryData();

        other.GetComponent<AliveEntity>().GetHealth.TakeHit(AttackData);
    }

    public void EnableCollider()
    {
        _sphereCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _sphereCollider.enabled = false;

        // if (_starterAssetsInputs != null)
        // {
        //     _cinemachineVirtualCamera.enabled = false;
        // }
    }

    public void Shoot()
    {
        if (AttackData.Target == null) return;

        var arrow = Instantiate(_aliveEntity.GetItemEquipper.GetProjectile.GetPrefab, _arrowSpawnPosition.position,
            Quaternion.identity);
        
        FindNecessaryData();
        
        arrow.GetComponent<ArrowMover>().SetInfoForArrow(_aliveEntity, AttackData);
    }

    private void FindNecessaryData()
    {
        AttackData.Damage = _aliveEntity.GetStat(Characteristics.Damage);
        AttackData.CriticalChance = _aliveEntity.GetStat(Characteristics.CriticalChance);
        AttackData.CriticalDamage = _aliveEntity.GetStat(Characteristics.CriticalDamage);
        AttackData.Damager = gameObject.transform.parent;
        OnHitTake?.Invoke(AttackData.Damager);
    }
}