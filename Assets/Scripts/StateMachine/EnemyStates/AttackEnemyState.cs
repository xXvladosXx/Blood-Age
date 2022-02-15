using System.Linq;
using Entity;
using StateMachine.BaseStates;
using StatsSystem;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    public class AttackEnemyState : AttackBaseState
    {
        private AliveEntity _target;
        private AliveEntity _entity;
        private StateDistanceConfiguration _stateDistanceConfiguration;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int ForceTransition = Animator.StringToHash("ForceTransition");

        public AttackEnemyState(StateDistanceConfiguration stateDistanceConfiguration)
        {
            _stateDistanceConfiguration = stateDistanceConfiguration;
        }

        public bool TriggeredByDamage { get; set; }

        public override void RunState(AliveEntity aliveEntity)
        {
            if (Health.IsDead())
            {
                ChaseSwitch();
                return;
            }
            Movement.Cancel();
            
            if (_target.GetHealth.IsDead() || !_stateDistanceConfiguration.IsInRange(_target, aliveEntity, ItemEquipper.GetAttackRange))
            {
                ChaseSwitch();
            }
            else
            {
                MakeAttack();
            }   
        }

        public override void StartState(float time)
        {
            _target = Entity.Targets.FirstOrDefault();
        }

        private void ChaseSwitch()
        {
            StateSwitcher.SwitchState<ChaseEnemyState>();
            Animator.SetBool(Attack, false);
            Animator.SetBool(ForceTransition, true);
        }

        private void MakeAttack()
        {
            AttackRegister.GetAttackData.PointTarget = _target;
            Entity.transform.LookAt(_target.transform);
            Animator.SetBool(Attack, true);
        }
    }
}