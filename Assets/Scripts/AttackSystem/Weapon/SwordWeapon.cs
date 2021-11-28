namespace AttackSystem.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace;
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