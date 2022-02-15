using System.Collections.Generic;
using AttackSystem.AttackMechanics;
using Entity;
using InventorySystem;
using RPGCharacterAnims.Actions;
using StateMachine.BaseStates;
using States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.PlayerStates
{
    public class AttackPlayerState : BasePlayerState
    {
        private float _attackDistance;
        
        private static readonly int MainAttack = Animator.StringToHash("Attack");
        private static readonly int ForceTransition = Animator.StringToHash("ForceTransition");
      
        public override void RunState(AliveEntity aliveEntity)
        {
            if(Health.IsDead())
                StateSwitcher.SwitchState<IdlePlayerState>();

            if (PointerOverUI()) return;
            _attackDistance = ItemEquipper.GetAttackRange;

            if (AttackRegister.GetAttackData.PointTarget.GetHealth.IsDead())
            {
                Animator.SetBool(MainAttack, false);
                StateSwitcher.SwitchState<IdlePlayerState>();
                return;
            }

            MakeAttack(aliveEntity);
            MakeCast(aliveEntity);
        }

        public override void StartState(float time)
        {
            
        }

        private void MakeAttack(AliveEntity aliveEntity)
        {
            Animator.SetBool(MainAttack, true);
            aliveEntity.transform.LookAt(AttackRegister.GetAttackData.PointTarget.transform);

            if (_attackDistance < Vector3.Distance(AttackRegister.GetAttackData.PointTarget.transform.position,
                    aliveEntity.transform.position))
            {
                SwitchToIdle();
            }
            
            if (StarterAssetsInputs.ButtonInput)
            {
                RaycastHit raycastHit;
                Physics.Raycast(AliveEntity.GetRay(), out raycastHit,Mathf.Infinity,  (1 << LayerMask.NameToLayer($"Entity") | 1 <<LayerMask.NameToLayer($"Default")));
                
                if (raycastHit.collider.TryGetComponent(out AliveEntity target)
                    && target != aliveEntity && !target.GetHealth.IsDead())
                {
                    if (target != AttackRegister.GetAttackData.PointTarget)
                    {
                        AttackRegister.GetAttackData.PointTarget.DisableOutline();
                    }
                    AttackRegister.GetAttackData.PointTarget = target;
                    AttackRegister.GetAttackData.PointTarget.EnableOutLine();
                }
                else{
                    Movement.StartMoveTo(raycastHit.point, 1f);
                    AttackRegister.GetAttackData.PointTarget.DisableOutline();
                    AttackRegister.GetAttackData.PointTarget = null;
                    SwitchToIdle();
                }
            }
            
        }

        
        private void SwitchToIdle()
        {
            Animator.SetBool(MainAttack, false);
            Animator.SetBool(ForceTransition, true);
            StateSwitcher.SwitchState<IdlePlayerState>();
        }
    }
}