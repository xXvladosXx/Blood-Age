namespace AttackSystem.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace;
    using InventorySystem;
    using UnityEditor.Animations;
    using UnityEngine;

    public abstract class StandardWeapon : StatModifItem
    {
        [SerializeField] protected bool _rightHanded;
        public bool IsRightHand => _rightHanded;
        
        [SerializeField] protected float _attackRange;
        public float GetAttackDistance => _attackRange;


        [SerializeField] protected AnimatorController _animatorController;
        
        [SerializeField] protected GameObject _weaponPrefab;
        [SerializeField] protected bool _isRanged;
        public GameObject GetPrefab => _weaponPrefab;
        public bool GetIsRanged => _isRanged;

        public abstract void EquipWeapon(Animator animator);
        
    }
}