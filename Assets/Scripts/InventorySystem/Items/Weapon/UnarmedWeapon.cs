using UnityEngine;

namespace InventorySystem.Items.Weapon
{
    [CreateAssetMenu (fileName = "Weapon", menuName = "Weapon/Unarmed")]

    public class UnarmedWeapon : StandardWeapon, IEquipable
    {
        public override void EquipWeapon(Animator animator)
        {
            animator.runtimeAnimatorController = _animatorController;
        }


        public InventoryItem GetItem()
        {
            return this;
        }

    }
}