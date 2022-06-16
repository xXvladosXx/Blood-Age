using System.Collections.Generic;
using System.Linq;
using Entity;
using StateMachine.BaseStates;
using States;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    public class IdleEnemyState : IdleBaseState
    {
        private Transform _pathToPatrol;
        private List<Transform> _pointsToPatrol = new List<Transform>();

        private AliveEntity _target;
        private StateDistanceConfiguration _stateDistanceConfiguration;

        private Vector3 _defaultStartPoint;
        private int _currentPointIndex;
        private bool _aggredByDamage;
        private bool _isAggred;

        public IdleEnemyState(Transform pathToPatrol, StateDistanceConfiguration stateDistanceConfiguration)
        {
            _pathToPatrol = pathToPatrol;
            _stateDistanceConfiguration = stateDistanceConfiguration;
        }

        public override void GetComponents(AliveEntity aliveEntity)
        {
            base.GetComponents(aliveEntity);
            if (_pathToPatrol == null)
            {
                _defaultStartPoint = aliveEntity.transform.position;
                return;
            }

            foreach (Transform point in _pathToPatrol)
            {
                _pointsToPatrol.Add(point);
            }

            _defaultStartPoint = aliveEntity.transform.position;
        }

        public override void RunState(AliveEntity aliveEntity)
        {
            if (aliveEntity.GetHealth.IsDead())
            {
                StateSwitcher.SwitchState<DeathBaseState>();
                return;
            }
            _target = Entity.Targets.FirstOrDefault();

            _isAggred = _stateDistanceConfiguration.IsInRange(_target, aliveEntity, _aggredByDamage
                ? _stateDistanceConfiguration.DamageChaseDistance
                : _stateDistanceConfiguration.ChaseDistance);

            if (_isAggred)
            {
                StateSwitcher.SwitchState<ChaseEnemyState>();
                _aggredByDamage = false;
            }
            else
            {
                GoToNextWaypoint(aliveEntity);
            }
        }

        public override void EndState(AliveEntity aliveEntity)
        {
            
        }

        public override bool CanBeChanged => true;


        public override void StartState(AliveEntity aliveEntity)
        {
            ResetAnimatorBools();
        }

        private void GoToNextWaypoint(AliveEntity aliveEntity)
        {
            if (_pointsToPatrol.Count == 0)
            {
                Movement.StartMoveTo(_defaultStartPoint, 0.2f);
                return;
            }

            Movement.StartMoveTo(_pointsToPatrol[_currentPointIndex].position, 0.2f);

            if (Vector3Int.RoundToInt((_pointsToPatrol[_currentPointIndex].position)) ==
                Vector3Int.RoundToInt((aliveEntity.transform.position)))
            {
                _currentPointIndex = (_currentPointIndex + 1) % _pointsToPatrol.Count;
            }
        }
    }
}