namespace AttackSystem.Weapon
{
    using System.Text;
    using DefaultNamespace;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Weapon/Staff")]
    public class StaffWeapon : StandardWeapon, IRangeable
    {
        public override void EquipWeapon(Animator animator)
        {
            animator.runtimeAnimatorController = _animatorController;
        }

        public ProjectileType GetProjectileType()
        {
            return ProjectileType.Stick;
        }

    }
}