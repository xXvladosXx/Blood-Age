namespace AttackSystem.Weapon
{
    using System;
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
    }
}