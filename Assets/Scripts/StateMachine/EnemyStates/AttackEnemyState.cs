using System.Linq;
using Entity;
using StateMachine.BaseStates;
using StatsSystem;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    public class AttackEnemyState : AttackBaseState
    {
        protected AliveEntity Target;
        protected StateDistanceConfiguration StateDistanceConfiguration;
        

        public AttackEnemyState(StateDistanceConfiguration stateDistanceConfiguration)
        {
            StateDistanceConfiguration = stateDistanceConfiguration;
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
            
            if (Target.GetHealth.IsDead() || !StateDistanceConfiguration.IsInRange(Target, aliveEntity, ItemEquipper.GetAttackRange))
            {
                ChaseSwitch();
            }
            else
            {
                MakeAttack();
            }   
        }

        public override void StartState(AliveEntity aliveEntity)
        {
            Target = Entity.Targets.FirstOrDefault();
        }

        public override bool CanBeChanged => true;

        private void ChaseSwitch()
        {
            if(Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8) return;
            
            StateSwitcher.SwitchState<ChaseEnemyState>();
            Animator.SetBool(Attack, false);
            Animator.SetBool(ForceTransition, true);
        }

        protected void MakeAttack()
        {
            AttackRegister.GetAttackData.PointTarget = Target;
            Entity.transform.LookAt(Target.transform);
            Animator.SetBool(Attack, true);
        }
    }
}