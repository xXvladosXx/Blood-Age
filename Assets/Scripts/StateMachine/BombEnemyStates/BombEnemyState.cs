using System.Collections.Generic;
using Entity;
using StateMachine.EnemyStates;
using UnityEngine;

namespace StateMachine.BombEnemyStates
{
    public class BombEnemyState : EnemyStateManager
    {
        [SerializeField] private GameObject _explosion;
        protected override void Awake()
        {
            AliveEntity = GetComponent<EnemyEntity>();

            if (StateDistanceConfiguration == null)
            {
                StateDistanceConfiguration =
                    Resources.Load<StateDistanceConfiguration>("DistanceConfigurationDefaultEnemy");
            }


            AllStates = new List<BaseState>
            {
                new IdleEnemyState(PathToPatrol, StateDistanceConfiguration),
                new ChaseEnemyState(StateDistanceConfiguration),
                new BombAttackEnemyState(StateDistanceConfiguration, _explosion),
                new StunEnemyState(),
                new TauntEnemyState()
            };
            CurrentBaseState = AllStates[0];
        }
    }
}