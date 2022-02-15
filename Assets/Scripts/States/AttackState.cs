using System.Collections;
using AttackSystem.AttackMechanics;
using Entity;
using SkillSystem;
using StateMachine;
using StateMachine.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace States
{
    public class AttackState : StateData
    {
        [SerializeField] private float _startAttackTime;
        [SerializeField] private float _endAttack;

        private AliveEntity _aliveEntity;
        private AttackRegister _attackRegister;

        private static readonly int WasRegistered = Animator.StringToHash("WasRegistered");
        private static readonly int Attack = Animator.StringToHash("Attack");


        public override void OnEnter(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            _aliveEntity = animator.GetComponent<AliveEntity>();
            _attackRegister = _aliveEntity.GetAttackRegister;
            animator.SetBool(WasRegistered, false);
        }


        public override void UpdateAbility(AnimatorState characterStateAnimator, Animator animator,
            AnimatorStateInfo stateInfo)
        {
            CheckCombat(animator, stateInfo);
        }

        private void CheckCombat(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame )
            {
                if (_attackRegister.GetAttackData.PointTarget != null)
                {
                    if (stateInfo.normalizedTime >= _startAttackTime && stateInfo.normalizedTime <= _endAttack) 
                    {
                        animator.SetBool(WasRegistered, true);
                    }
                }
            }
        }

        public override void OnExit(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(WasRegistered, false);
            animator.SetBool(Attack,false);
        }
    }
}