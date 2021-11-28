using System;
using System.Collections;
using System.Collections.Generic;
using AttackSystem;
using Cinemachine;
using DefaultNamespace;
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
    private StarterAssetsInputs _starterAssetsInputs;
    private Health _health;
    private ItemEquipper _itemEquipper;
    private FindStats _findStats;

    public event Action<Transform> OnHitTake;


    private void Awake()
    {
        AttackData = new AttackData();

        _sphereCollider = GetComponent<SphereCollider>();
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        _health = GetComponentInParent<Health>();
        _itemEquipper = GetComponentInParent<ItemEquipper>();
        _findStats = GetComponentInParent<FindStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() == null) return;
        if (other.GetComponent<Health>() == _health) return;

        if (_starterAssetsInputs != null && AttackData.HeavyAttack)
        {
            _cinemachineVirtualCamera.enabled = true;
        }

        FindNecessaryData();

        other.GetComponent<Health>().TakeHit(AttackData);
    }

    public void EnableCollider()
    {
        _sphereCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _sphereCollider.enabled = false;

        if (_starterAssetsInputs != null)
        {
            _cinemachineVirtualCamera.enabled = false;
        }
    }

    public void Shoot()
    {
        if (AttackData.Target == null) return;

        var arrow = Instantiate(_itemEquipper.GetProjectile.GetPrefab, _arrowSpawnPosition.position,
            Quaternion.identity);
        
        FindNecessaryData();
        
        arrow.GetComponent<ArrowMover>().SetInfoForArrow(_health, AttackData);
    }

    private void FindNecessaryData()
    {
        AttackData.Damage = _findStats.GetStat(_findStats.GetClass, Characteristics.Damage);
        AttackData.CriticalChance = _findStats.GetStat(_findStats.GetClass, Characteristics.CriticalChance);
        AttackData.CriticalDamage = _findStats.GetStat(_findStats.GetClass, Characteristics.CriticalDamage);
        AttackData.Damager = gameObject.transform.parent;
        OnHitTake?.Invoke(AttackData.Damager);
      
    }
}