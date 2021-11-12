namespace AttackSystem.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace;
    using InventorySystem;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    [CreateAssetMenu (fileName = "Weapon", menuName = "Weapon/Bow")]

    public class BowWeapon : StandardWeapon, IModifier, IRangeable
    {
        [Range(1, 10)]
        [SerializeField] private float _attackSpeed;

        private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");

        public override void EquipWeapon(Animator animator)
        {
            animator.runtimeAnimatorController = _animatorController;
            animator.SetFloat(AttackSpeed, _attackSpeed);
        }

        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IBonus BonusTo(Characteristics characteristics)
            {
                return characteristics switch
                {
                    Characteristics.Damage => new DamageBonus(_damage),
                    Characteristics.Health => new HealthBonus(_healthBonus),
                    _ => throw new ArgumentOutOfRangeException(nameof(characteristics), characteristics, null)
                };
            }
            
            return characteristics.Select(BonusTo);
        }

        public ProjectileType GetProjectileType()
        {
            return ProjectileType.Arrow;
        }
    }

    
}