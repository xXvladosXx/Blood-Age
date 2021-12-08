namespace DefaultNamespace
{
    using System;
    using System.Collections.Generic;
    using AI;
    using DefaultNamespace.Entity;
    using InventorySystem;
    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.InputSystem;

    [CreateAssetMenu (fileName = ("Movement"), menuName = "State/Movement")]
    public abstract class IdleMovementState : StateData
    {
        [SerializeField] protected bool manualMovement = false;
        protected float DistanceToAttack;
        
        protected Movement Movement;
        protected AttackRegistrator AttackRegistrator;
        protected AliveEntity AliveEntity;
        
        protected static readonly int ForceTransition = Animator.StringToHash("ForceTransition");
        protected static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Roll = Animator.StringToHash("Roll");
        private static readonly int Canceled = Animator.StringToHash("Canceled");

        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(Canceled, false);
            AttackRegistrator = animator.GetComponentInChildren<AttackRegistrator>();
            Movement = animator.GetComponent<Movement>();
            AliveEntity = animator.GetComponent<AliveEntity>();
        }

        public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            if(Movement != null)
                Movement.Cancel();
            
            animator.SetBool(ForceTransition, false);
            animator.SetBool(Roll, false);
        }
    }
}