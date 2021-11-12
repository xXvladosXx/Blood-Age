namespace AttackSystem.AttackMechanics
{
    using System;
    using AttackSystem.Weapon;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class BowAnimatorController : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void DrawRope()
        {
            _animator.Play("BowDrawAnimation");
        }
    }
}