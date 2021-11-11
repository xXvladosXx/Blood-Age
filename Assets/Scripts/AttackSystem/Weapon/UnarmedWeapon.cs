namespace AttackSystem.Weapon
{
    using UnityEngine;

    [CreateAssetMenu (fileName = "Weapon", menuName = "Weapon/Unarmed")]

    public class UnarmedWeapon : StandardWeapon
    {
        public override void EquipWeapon(Animator animator)
        {
            animator.runtimeAnimatorController = _animatorController;
        }
    }
}