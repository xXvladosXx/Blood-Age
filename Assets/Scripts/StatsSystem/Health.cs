using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.MouseSystem;
using InventorySystem;
using SkillSystem.MainComponents;
using StatsSystem;
using UnityEngine;
using Random = UnityEngine.Random;


public class Health
{
    private float _currentHealth;
    private bool _isDead;
    public event Action<Transform> OnTakeHit;
    public event Action OnHeavyAttackHit;
    public event Action OnDie;


    public Health(float healthPoints)
    {
        _currentHealth = healthPoints;
    }

    public void RenewHealthPoints(float healthPoints)
    {
        _currentHealth = healthPoints;
    }

    public void TakeHit(AttackData attackData)
    {
        float currentChance = Random.Range(0, 100);
        attackData.CriticalChance = 15;
        attackData.CriticalDamage = 110;

        _currentHealth = Mathf.Clamp(_currentHealth, 0, _currentHealth);

        if (attackData.Damager.GetComponent<StarterAssetsInputs>() != null)
        {
            Debug.Log("Damaged");
        }
        
        if (attackData.HeavyAttack)
            OnHeavyAttackHit?.Invoke();

        if (_currentHealth <= 0)
        {
            OnDie?.Invoke();
            _isDead = true;
            Debug.Log("Died");
        }
        
        OnTakeHit?.Invoke(attackData.Damager);
    }

    public bool IsDead() => _isDead;
}
