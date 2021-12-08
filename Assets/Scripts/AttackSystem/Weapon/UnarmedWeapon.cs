namespace AttackSystem.Weapon
{
    using InventorySystem;
    using UnityEngine;

    [CreateAssetMenu (fileName = "Weapon", menuName = "Weapon/Unarmed")]

    public class UnarmedWeapon : StandardWeapon, IEquipable
    {
        public override void EquipWeapon(Animator animator)
        {
            animator.runtimeAnimatorController = _animatorController;
        }


        public Item GetItem()
        {
            return this;
        }
    }
}