namespace AttackSystem.Weapon
{
    using DefaultNamespace;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Weapon/Staff")]
    public class StaffWeapon : StandardWeapon, IModifier, IRangeable
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
            return ProjectileType.Stick;
        }
    }
}