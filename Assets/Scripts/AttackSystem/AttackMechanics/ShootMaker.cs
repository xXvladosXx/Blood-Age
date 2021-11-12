namespace AttackSystem.Weapon
{
    using System;
    using AttackSystem.AttackMechanics;
    using UnityEngine;

    public class ShootMaker : MonoBehaviour
    {
        private AttackRegistrator _attackRegistrator;
        
        private void Awake()
        {
            _attackRegistrator = GetComponentInChildren<AttackRegistrator>();
        }

        public void Shoot()
        {
            _attackRegistrator.Shoot();
        }

        public void StartDraw()
        {
            BowAnimatorController bowAnimatorController = GetComponentInChildren<BowAnimatorController>();
            
            print(bowAnimatorController);
            bowAnimatorController.DrawRope();
        }
    }
}