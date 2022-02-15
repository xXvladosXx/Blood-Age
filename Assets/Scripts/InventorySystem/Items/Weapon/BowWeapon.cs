using AttackSystem.Weapon;
using UnityEngine;

namespace InventorySystem.Items.Weapon
{
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


        public InventoryItem GetItem()
        {
            return this;
        }
    }

    
}