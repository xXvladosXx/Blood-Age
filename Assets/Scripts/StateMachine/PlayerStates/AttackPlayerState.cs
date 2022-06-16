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
    public class AttackPlayerState : AttackBaseState
    {
        private float _attackDistance;
        private PlayerInputs _playerInputs;
        private PlayerEntity _playerEntity;

        private static readonly int MainAttack = Animator.StringToHash("Attack");
        private static readonly int ForceTransition = Animator.StringToHash("ForceTransition");

        public override void GetComponents(AliveEntity aliveEntity)
        {
            base.GetComponents(aliveEntity);
            _playerEntity = Entity as PlayerEntity;
            _playerInputs = _playerEntity.GetComponent<PlayerInputs>();
        }

        public override void StartState(AliveEntity aliveEntity)
        {
            base.StartState(aliveEntity);
            Animator.SetBool(ForceTransition, false);
        }

        public override void RunState(AliveEntity aliveEntity)
        {
            if (_playerEntity.PointerOverUI()) return;
            _attackDistance = ItemEquipper.GetAttackRange;

            if (AttackRegister.GetAttackData.PointTarget.GetHealth.IsDead())
            {
                SwitchToIdle();
                return;
            }

            MakeAttack(aliveEntity);
            MakeCast(aliveEntity);
        }
        

        public override bool CanBeChanged => true;

        private void MakeAttack(AliveEntity aliveEntity)
        {
            Animator.SetBool(MainAttack, true);
            aliveEntity.transform.LookAt(AttackRegister.GetAttackData.PointTarget.transform);

            if (_attackDistance < Vector3.Distance(AttackRegister.GetAttackData.PointTarget.transform.position,
                    aliveEntity.transform.position))
            {
                SwitchToIdle();
            }

            if (_playerInputs.ButtonInput)
            {
                RaycastHit raycastHit;
                Physics.Raycast(_playerEntity.GetRay(), out raycastHit, Mathf.Infinity,
                    (1 << LayerMask.NameToLayer($"Entity") | 1 << LayerMask.NameToLayer($"Default")));

                if (raycastHit.collider.TryGetComponent(out AliveEntity target)
                    && target != aliveEntity && !target.GetHealth.IsDead())
                {
                    if (target != AttackRegister.GetAttackData.PointTarget)
                    {
                        AttackRegister.GetAttackData.PointTarget.DisableOutline();
                        SwitchToIdle();
                    }

                    AttackRegister.GetAttackData.PointTarget = target;
                }
                else
                {
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
            StateSwitcher.SwitchState<IdleBaseState>();
        }
        
        private void MakeCast(AliveEntity aliveEntity)
        {
            for(int i = 0; i < 3; i++) {
                if(Keyboard.current[(Key) ((int)Key.Digit1 + i)].wasPressedThisFrame) {
                    CastSkillOnIndex(aliveEntity, i);
                }
            }
        }
        
        private void CastSkillOnIndex(AliveEntity aliveEntity, int index)
        {
            var skillCast = StateSwitcher.SwitchState<CastPlayerState>();
            if (skillCast.CastSkill(index, aliveEntity))
            {
                Animator.SetBool(MainAttack, false);
                Animator.SetBool(ForceTransition, true);
            }
        }
    }
}