namespace DefaultNamespace
{
    using System;
    using System.Collections.Generic;
    using AI;
    using InventorySystem;
    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.InputSystem;

    [CreateAssetMenu (fileName = ("Movement"), menuName = "State/Movement")]
    public abstract class IdleMovementState : StateData
    {
        [SerializeField] protected bool _manualMovement = false;
        protected float _distanceToAttack;
        
        protected Movement _movement;
        protected AttackRegistrator _attackRegistrator;
        protected ItemEquipper _itemEquipper;
        
        protected static readonly int ForceTransition = Animator.StringToHash("ForceTransition");
        protected static readonly int Attack = Animator.StringToHash("Attack");

        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            _attackRegistrator = animator.GetComponentInChildren<AttackRegistrator>();
            _movement = animator.GetComponent<Movement>();
           
            animator.SetBool(ForceTransition, false);
            
            _itemEquipper = animator.GetComponent<ItemEquipper>();
        }

        public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(ForceTransition, false);
        }
    }
}