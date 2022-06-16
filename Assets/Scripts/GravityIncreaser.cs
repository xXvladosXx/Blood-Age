using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityIncreaser : MonoBehaviour
{
    [SerializeField] private float _gravityMultiplier;

    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        if(_rigidbody != null)
            _rigidbody.AddForce(Vector3.down * _gravityMultiplier * _rigidbody.mass);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is TerrainCollider terrainCollider)
            Destroy(_rigidbody);
    }
}
