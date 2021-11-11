namespace AttackSystem.Weapon
{
    using DefaultNamespace;
    using InventorySystem;
    using UnityEngine;

    [CreateAssetMenu (fileName = "Weapon", menuName = "Weapon/Bow")]

    public class BowWeapon : StandardWeapon, IModifier, IRangeable
    {
        public override void EquipWeapon(Animator animator)
        {
            animator.runtimeAnimatorController = _animatorController;
        }

        public float AddBonus(Characteristics characteristics)
        {
            if(characteristics == Characteristics.Damage)
                return _damage;

            return 0;
        }

        public ProjectileType GetProjectileType()
        {
            return ProjectileType.Arrow;
        }
    }

    
}