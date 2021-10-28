using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DefaultNamespace.MouseSystem;
using UnityEngine;
using UnityEngine.VFX;

public class AttackRegistrator : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private float _timeToDisable = .5f;
    [SerializeField] private GameObject _visualEffect;
    
    public AttackData AttackData;
    private SphereCollider _sphereCollider;

    private float _startTime = 0;
    private bool _hitWasMade;

    private void Awake()
    {
        AttackData = new AttackData();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            //   _cinemachineVirtualCamera.enabled = true;

            var visualEffect = Instantiate(_visualEffect, other.transform.position, Quaternion.identity);
            
            other.GetComponent<Enemy>().TakeHit();
            
            Destroy(visualEffect, .7f);
        }
    }

    public void EnableCollider()
    {
        _sphereCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _sphereCollider.enabled = false;
        //_cinemachineVirtualCamera.enabled = false;
    }
}
