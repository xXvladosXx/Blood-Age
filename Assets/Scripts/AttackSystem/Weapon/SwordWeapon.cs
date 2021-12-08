namespace AttackSystem.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace;
    using InventorySystem;
    using UnityEngine;

    [CreateAssetMenu (fileName = "Weapon", menuName = "Weapon/Sword")]
    public class SwordWeapon : StandardWeapon, IEquipable
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