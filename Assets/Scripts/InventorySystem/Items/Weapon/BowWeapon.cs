namespace AttackSystem.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DefaultNamespace;
    using InventorySystem;
    using InventorySystem.Items;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    [CreateAssetMenu (fileName = "Weapon", menuName = "Weapon/Bow")]

    public class BowWeapon : StandardWeapon, IRangeable, IEquipable
    {
        [Range(1, 10)]
        [SerializeField] private float _attackSpeed;

        private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");

        public override void EquipWeapon(Animator animator)
        {
            animator.runtimeAnimatorController = _animatorController;
            animator.SetFloat(AttackSpeed, _attackSpeed);
        }

        
        public ProjectileType GetProjectileType()
        {
            return ProjectileType.Arrow;
        }


        public Item GetItem()
        {
            return this;
        }


        public Rarity GetRarity => _rarity;
    }

    
}