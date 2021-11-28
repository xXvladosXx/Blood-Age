using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.MouseSystem;
using InventorySystem;
using SkillSystem.MainComponents;
using StatsSystem;
using UnityEngine;

[RequireComponent(typeof(FindStats))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ItemEquipper))]

public class Health : MonoBehaviour
{
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _currentDamage;
    
    private Animator _animator;
    private FindStats _findStats;
    private ItemEquipper _itemEquipper;
    private BuffApplier _buffApplier;
    
    public event Action<Transform> OnTakeHit;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _findStats = GetComponent<FindStats>();
        _itemEquipper = GetComponent<ItemEquipper>();
        _buffApplier = GetComponent<BuffApplier>();
        
        FindHealthStat();

        _itemEquipper.OnEquipmentChanged += FindHealthStat;
        _buffApplier.OnBonusAdded += FindHealthStat;
    }

    private void FindHealthStat()
    {
        _currentHealth = _findStats.GetStat(_findStats.GetClass, Characteristics.Health);
        _currentDamage = (_findStats.GetStat(_findStats.GetClass, Characteristics.Damage));
    }

    public void TakeHit(AttackData attackData)
    {
        if(attackData.HeavyAttack)
            _animator.Play("Falling");

        OnTakeHit?.Invoke(attackData.Damager);
        print("Damaged " + gameObject);
    }
}
