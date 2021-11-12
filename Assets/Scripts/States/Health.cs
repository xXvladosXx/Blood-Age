using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.MouseSystem;
using UnityEngine;

[RequireComponent(typeof(FindStats))]
public class Health : MonoBehaviour, IRaycastable
{
    private Animator _animator;
    private FindStats _findStats;
    [SerializeField] private float _currentHealth;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _findStats = GetComponent<FindStats>();
        
        _currentHealth = _findStats.GetStat(_findStats.GetClass, Characteristics.Health);
    }

    public bool HandleRaycast(Transform gameObject)
    {
        return false;
    }

    public void TakeHit(AttackData attackData)
    {
        if(attackData.HeavyAttack)
            _animator.Play("Falling");
        
        
        print(attackData.Damage);
    }
}
