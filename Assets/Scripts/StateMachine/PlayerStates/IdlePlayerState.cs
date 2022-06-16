using System.Collections.Generic;
using AttackSystem.AttackMechanics;
using DialogueSystem;
using DialogueSystem.AIDialogue;
using Entity;
using LootSystem;
using QuestSystem;
using SceneSystem;
using StateMachine.BaseStates;
using StateMachine.EnemyStates;
using States;
using UI.Stats;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace StateMachine.PlayerStates
{
    public class IdlePlayerState : IdleBaseState
    {
        private AIConversant _lastClickedConversant;
        private ItemPickUp _lastClickedLootObject;
        
        private PlayerInputs _playerInputs;
        private PlayerEntity _playerEntity;
        private PlayerQuestList _playerQuests;

        private Portal _lastClickedPortal;
        private bool _transitionStarted;

        private float _distanceToAttack;
        private float _distanceToSpeak = 3f;
        private float _distanceToPickUp = 1f;
        private float _distanceToTeleport = 2f;
        private float _timeToClick = .5f;
        private float _clicking;


        public override void GetComponents(AliveEntity aliveEntity)
        {
            base.GetComponents(aliveEntity);
            _playerEntity = aliveEntity as PlayerEntity;
            _playerInputs = _playerEntity.GetComponent<PlayerInputs>();
            _playerQuests = _playerEntity.GetComponent<PlayerQuestList>();
        }

        public override void RunState(AliveEntity aliveEntity)
        {
            if(Health.IsDead()) 
                StateSwitcher.SwitchState<DeathBaseState>();
            
            _distanceToAttack = aliveEntity.GetItemEquipper.GetAttackRange;
            if (_playerInputs.enabled == false) return;

            PlayerInput(aliveEntity);
        }



        public override bool CanBeChanged => true;


        private void PlayerInput(AliveEntity aliveEntity)
        {
            if (StateSwitcher.GetCurrentState is CastPlayerState) return;

            MakeCast(aliveEntity);

            if (_playerEntity.PointerOverUI()) return;
            MovementInput(aliveEntity);
            DialogueBehaviour(aliveEntity);
            TeleportBehaviour(aliveEntity);
            AttackBehaviour(aliveEntity);
            PickingItemBehaviour(aliveEntity);
        }

        private void TeleportBehaviour(AliveEntity aliveEntity)
        {
            if (_lastClickedPortal == null) return;

            var enoughToStartNewState = EnoughDistanceToTarget(aliveEntity,
                _lastClickedPortal.transform, _distanceToTeleport);

            if (enoughToStartNewState)
            {
                _playerQuests.CheckPortalQuests();
                _lastClickedPortal.StartTransition(aliveEntity);
                StateSwitcher.SwitchState<TeleportPlayerState>();
            }
        }

        private void MovementInput(AliveEntity aliveEntity)
        {
            
            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(_playerEntity.GetRay(), out raycastHit, Mathf.Infinity);
            
            if (!hasHit) return;

            if (_playerInputs.ButtonInput)
            {
                _clicking += Time.deltaTime;
                
                if(raycastHit.collider.gameObject == aliveEntity.gameObject) return;

                if (raycastHit.collider.TryGetComponent(out AliveEntity target)
                    && target != aliveEntity && !target.GetHealth.IsDead() &&
                    target.GetComponent<EnemyStateManager>() != null)
                {
                    AttackRegister.GetAttackData.PointTarget = target;
                    HealthBarEntity.Instance.ShowHealth(target);
                    AttackRegister.GetAttackData.PointTarget.EnableOutLine();
                }
                else
                {
                    if(_lastClickedConversant != null)
                        _lastClickedConversant.DisableOutline();
                    if(AttackRegister.GetAttackData.PointTarget != null)
                        AttackRegister.GetAttackData.PointTarget .DisableOutline();
                    
                    Movement.StartMoveTo(raycastHit.point, 1f);
                    AttackRegister.GetAttackData.PointTarget = null;
                    _lastClickedConversant = null;
                    _lastClickedLootObject = null;
                    _lastClickedPortal = null;
                    
                    HealthBarEntity.Instance.HideHealth();
                }
                if(_clicking < _timeToClick && raycastHit.collider.TryGetComponent(out AIConversant aiConversant))
                {
                    _lastClickedConversant = aiConversant;
                    _lastClickedConversant.EnableOutLine();
                }
                else if (_clicking < _timeToClick &&raycastHit.collider.TryGetComponent(out ItemPickUp lootObject))
                {
                    _lastClickedLootObject = lootObject;
                }else if (_clicking < _timeToClick &&raycastHit.collider.TryGetComponent(out Portal portal))
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
        
        private void MakeCast(AliveEntity aliveEntity)
        {
            if(GameZoneManager.Instance.GetGameZone == GameZoneManager.GameZone.Savezone) return;
            
            for(int i = 0; i < 3; i++) {
                if(Keyboard.current[(Key) ((int)Key.Digit1 + i)].wasPressedThisFrame) {
                    CastSkillOnIndex(aliveEntity, i);
                }
            }
        }
        
        private void CastSkillOnIndex(AliveEntity aliveEntity, int index)
        {
            var skillCast = StateSwitcher.SwitchState<CastPlayerState>();
            skillCast.CastSkill(index, aliveEntity);
        }
    }
}