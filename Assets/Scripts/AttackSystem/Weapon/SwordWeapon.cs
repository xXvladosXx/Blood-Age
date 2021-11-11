namespace AttackSystem.Weapon
{
    using UnityEngine;

    [CreateAssetMenu (fileName = "Weapon", menuName = "Weapon/Sword")]
    public class SwordWeapon : StandardWeapon
    {
        public override void EquipWeapon(Animator animator)
        {
            animator.runtimeAnimatorController = _animatorController;
        }
    }
}