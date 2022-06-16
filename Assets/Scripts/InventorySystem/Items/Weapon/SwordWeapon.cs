using UnityEngine;

namespace InventorySystem.Items.Weapon
{
    [CreateAssetMenu (fileName = "Weapon", menuName = "Inventory/Weapon/Sword")]
    public class SwordWeapon : StandardWeapon, IEquipable
    {
        public override void EquipWeapon(Animator animator)
        {
            if(animator == null) return;
            
            animator.runtimeAnimatorController = _animatorController;
        }

        public InventoryItem GetItem()
        {
            return this;
        }
        
    }
}