using System.Collections.Generic;
using AttackSystem.AttackMechanics;
using DialogueSystem;
using DialogueSystem.AIDialogue;
using Entity;
using LootSystem;
using SceneSystem;
using StateMachine.BaseStates;
using StateMachine.EnemyStates;
using States;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace StateMachine.PlayerStates
{
    public class IdlePlayerState : IdleBaseState
    {
        private AIConversant _lastClickedConversant;
        private ItemPickUp _lastClickedLootObject;
        private StarterAssetsInputs _starterAssetsInputs;
        private Portal _lastClickedPortal;
        private bool _transitionStarted;

        private float _distanceToAttack;
        private float _distanceToSpeak = 3f;
        private float _distanceToPickUp = 1f;
        private float _timeToClick = .1f;
        private float _clicking;

        private static readonly int ForceTransition = Animator.StringToHash("ForceTransition");

        public override void GetComponents(AliveEntity aliveEntity)
        {
            base.GetComponents(aliveEntity);
            _starterAssetsInputs = Movement.GetComponent<StarterAssetsInputs>();
        }

        public override void RunState(AliveEntity aliveEntity)
        {
            _distanceToAttack = aliveEntity.GetItemEquipper.GetAttackRange;
            if(Health.IsDead())
                StateSwitcher.SwitchState<DeathPlayerState>();

            if (_starterAssetsInputs.enabled == false) return;

            Animator.SetBool(ForceTransition, false);

            if (aliveEntity.GetHealth.IsDead()) return;

            PlayerCastSkillInput(aliveEntity);
        }

        public override void StartState(float time)
        {
        }

        private void PlayerCastSkillInput(AliveEntity aliveEntity)
        {
            if (StateSwitcher.GetCurrentState is CastPlayerState) return;

            MakeCast(aliveEntity);

            if (PointerOverUI()) return;
            MovementInput(aliveEntity);
            DialogueBehaviour(aliveEntity);
            AttackBehaviour(aliveEntity);
            PickingItemBehaviour(aliveEntity);
            TeleportBehaviour(aliveEntity);
        }

        private void TeleportBehaviour(AliveEntity aliveEntity)
        {
            if (_lastClickedPortal == null) return;

            var enoughToStartNewState = EnoughDistanceToTarget(aliveEntity,
                _lastClickedPortal.transform, _distanceToPickUp);

            if (enoughToStartNewState)
            {
                _lastClickedPortal.StartTransition();
                StateSwitcher.SwitchState<TeleportPlayerState>();
            }
        }

        private void MovementInput(AliveEntity aliveEntity)
        {
            if (_starterAssetsInputs.ButtonInput)
            {
                _clicking += Time.deltaTime;
                var player = aliveEntity as PlayerEntity;
                RaycastHit raycastHit;
                bool hasHit = Physics.Raycast(player.GetRay(), out raycastHit, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Unwalkable")));
                if (!hasHit) return;
                if(raycastHit.collider.gameObject == aliveEntity.gameObject) return;

                if (raycastHit.collider.TryGetComponent(out AliveEntity target)
                    && target != aliveEntity && !target.GetHealth.IsDead() &&
                    target.GetComponent<EnemyStateManager>() != null)
                {
                    AttackRegister.GetAttackData.PointTarget = target;
                }
                else
                {
                    Movement.StartMoveTo(raycastHit.point, 1f);
                    AttackRegister.GetAttackData.PointTarget = null;
                    _lastClickedConversant = null;
                    _lastClickedLootObject = null;
                }
                if(_clicking < _timeToClick && raycastHit.collider.TryGetComponent(out AIConversant aiConversant))
                {
                    _lastClickedConversant = aiConversant;
                }
                else if (_clicking < _timeToClick &&raycastHit.collider.TryGetComponent(out ItemPickUp lootObject))
                {
                    _lastClickedLootObject = lootObject;
                }else if (_clicking < _timeToClick && raycastHit.collider.TryGetComponent(out Portal portal))
                {
                    _lastClickedPortal = portal;
                }
            }
            else
            {
                _clicking = 0;
            }
        }

        private void DialogueBehaviour(AliveEntity aliveEntity)
        {
            if (_lastClickedConversant == null) return;

            var enoughToStartNewState = EnoughDistanceToTarget(aliveEntity, _lastClickedConversant.transform, _distanceToSpeak);

            if (enoughToStartNewState)
            {
                StateSwitcher.SwitchState<DialoguePlayerState>().StartDialogue(_lastClickedConversant);
                _lastClickedConversant = null;
            }
        }

        private void AttackBehaviour(AliveEntity aliveEntity)
        {
            if (AttackRegister.GetAttackData.PointTarget == null) return;
            if (AttackRegister.GetAttackData.PointTarget.GetHealth.IsDead()) return;

            var enoughToStartNewState = EnoughDistanceToTarget(aliveEntity,
                AttackRegister.GetAttackData.PointTarget.transform, _distanceToAttack);

            if (enoughToStartNewState)
            {
                AttackRegister.GetAttackData.PointTarget.EnableOutLine();
                StateSwitcher.SwitchState<AttackPlayerState>();
            }
        }

        private void PickingItemBehaviour(AliveEntity aliveEntity)
        {
            if (_lastClickedLootObject == null) return;

            var enoughToStartNewState = EnoughDistanceToTarget(aliveEntity,
                _lastClickedLootObject.transform, _distanceToPickUp);

            if (enoughToStartNewState)
            {
                StateSwitcher.SwitchState<PickingPlayerState>().GoToPickUp(_lastClickedLootObject);
            }
        }

        private bool EnoughDistanceToTarget(AliveEntity user, Transform target, float distance)
        {
            var distanceToTargetSec =
                Vector3.Distance(user.transform.position,
                    target.position);

            if (distanceToTargetSec < distance)
            {
                Movement.Cancel();
                return true;
            }

            Movement.StartMoveTo(target.position, 1f);
            return false;
        }
    }
}