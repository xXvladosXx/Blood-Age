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
    [SerializeField] private float _timeToDisable = .5f;
    [SerializeField] private GameObject _visualEffect;
    [SerializeField] private Transform _arrowSpawnPosition;
    
    public AttackData AttackData;
    private SphereCollider _sphereCollider;
    private StarterAssetsInputs _starterAssetsInputs;
    private Health _health;
    private ItemEquipper _itemEquipper;
    private ClassChooser _classChooser;
    
    private float _startTime = 0;
    private bool _hitWasMade;

    private void Awake()
    {
        AttackData = new AttackData();
        _sphereCollider = GetComponent<SphereCollider>();
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        _health = GetComponentInParent<Health>();
        _itemEquipper = GetComponentInParent<ItemEquipper>();
        _classChooser = GetComponentInParent<ClassChooser>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() == null) return;
        if(other.GetComponent<Health>() == _health) return;
        
        if (_starterAssetsInputs != null && AttackData.HeavyAttack)
        {
            _cinemachineVirtualCamera.enabled = true;
        }

        AttackData.Damage = _classChooser.GetStat(_classChooser.GetClass, Characteristics.Damage);
        
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
        var arrow = Instantiate(_itemEquipper.GetProjectile.GetPrefab, _arrowSpawnPosition.position, Quaternion.identity);
        
        if(AttackData.Target == null) return;
        arrow.AddComponent<ArrowMover>().SetInfoForArrow(15, _health, AttackData.Target, 
            _classChooser.GetStat(_classChooser.GetClass, Characteristics.Damage));
    }
}
