using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.MouseSystem;
using UnityEngine;

[RequireComponent(typeof(ClassChooser))]
public class Health : MonoBehaviour, IRaycastable
{
    private Animator _animator;
    private ClassChooser _classChooser;
    [SerializeField] private float _currentHealth;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _classChooser = GetComponent<ClassChooser>();
        
        _currentHealth = _classChooser.GetStat(_classChooser.GetClass, Characteristics.Health);
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
