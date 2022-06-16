using System.Linq;
using Entity;
using InventorySystem;
using StateMachine.BaseStates;
using States;
using StatsSystem;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    public class ChaseEnemyState : BaseState
    {
        private AliveEntity _entity;
        private Movement _movement;
        private IStateSwitcher _stateSwitcher;
        private ItemEquipper _itemEquipper;
        private AliveEntity _target;
        private StateDistanceConfiguration _stateDistanceConfiguration;
        private Animator _animator;
        private bool _aggredByDamage;
        private static readonly int ForceTransition = Animator.StringToHash("ForceTransition");

        public ChaseEnemyState(StateDistanceConfiguration stateDistanceConfiguration)
        {
            _stateDistanceConfiguration = stateDistanceConfiguration;
        }

        public override void GetComponents(AliveEntity aliveEntity)
        {
            _entity = aliveEntity;
            _movement = aliveEntity.GetComponent<Movement>();
            _stateSwitcher = aliveEntity.GetComponent<IStateSwitcher>();
            _itemEquipper = aliveEntity.GetItemEquipper;
            _target = aliveEntity.Targets.FirstOrDefault();
            _animator = aliveEntity.GetComponent<Animator>();
            
            aliveEntity.GetHealth.OnTakeHit += o => _aggredByDamage = true;
        }

        public override void EndState(AliveEntity aliveEntity)
        {
            
        }

        public override bool CanBeChanged => true;
        
        public override void RunState(AliveEntity aliveEntity)
        {
            if (aliveEntity.GetHealth.IsDead())
            {
                SwitchToIdle();
                return;
            }
            
            _animator.SetBool(ForceTransition, false);
            
            if(_target == null) return;
            
            if (_stateDistanceConfiguration.IsInRange(aliveEntity, _target,
                TriggeredByDamage
                    ? _stateDistanceConfiguration.DamageChaseDistance
                    : _stateDistanceConfiguration.ChaseDistance))
            {
                if (_stateDistanceConfiguration.IsInRange(aliveEntity, _target, _itemEquipper.GetAttackRange))
                {
                    _movement.Cancel();
                    _stateSwitcher.SwitchState<AttackBaseState>();
                }
                else
                {
                    _movement.StartMoveTo(_target.transform.position, 0.7f);
                }
            }
            else
            {
                SwitchToIdle();
            }
        }

        public override void StartState(AliveEntity aliveEntity)
        {
            _target = _entity.Targets.FirstOrDefault();
        }

        private void SwitchToIdle()
        {
            _movement.Cancel();
            _stateSwitcher.SwitchState<IdleBaseState>();
            TriggeredByDamage = false;
        }

        public bool TriggeredByDamage { get; set; }
        
    }
}