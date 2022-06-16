using System.Linq;
using DG.Tweening;
using Entity;
using StateMachine.BaseStates;
using StateMachine.EnemyStates;
using StatsSystem;
using UnityEngine;

namespace StateMachine.BombEnemyStates
{
    public class BombAttackEnemyState : AttackEnemyState
    {
        private Tween _explosionTween;
        private float _timeToExplode =.3f;
        private float _timeToDestroyExplosion = 3f;
        private GameObject _explosion;
        
        public BombAttackEnemyState(StateDistanceConfiguration stateDistanceConfiguration, GameObject explosion) : base(stateDistanceConfiguration)
        {
            StateDistanceConfiguration = stateDistanceConfiguration;
            _explosion = explosion;
        }

        public override void StartState(AliveEntity aliveEntity)
        {
            base.StartState(aliveEntity);
            Movement.Cancel();
            
            if (StateDistanceConfiguration.IsInRange(Target, aliveEntity, ItemEquipper.GetAttackRange))
            {
                Target.GetHealth.TakeHit(aliveEntity.GetAttackRegister.CalculateAttackData(FindStat, aliveEntity, ItemEquipper));
                aliveEntity.GetHealth.TakeHit(aliveEntity.GetAttackRegister.CalculateAttackData(FindStat, aliveEntity, ItemEquipper));
            }
        }
        
    }
}