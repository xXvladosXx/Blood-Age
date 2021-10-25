using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DefaultNamespace.MouseSystem;
using UnityEngine;

public class AttackMaker : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private float _timeToDisable = .5f;
    
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
            _cinemachineVirtualCamera.enabled = true;

            other.GetComponent<Enemy>().TakeHit();
        }
    }

    public void EnableCollider()
    {
        _sphereCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _sphereCollider.enabled = false;
        _cinemachineVirtualCamera.enabled = false;
    }
}
