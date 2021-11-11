namespace DefaultNamespace
{
    using System.Collections;
    using System.Collections.Generic;
    using AI;
    using UnityEngine;

    [CreateAssetMenu(fileName = ("Movement"), menuName = "State/AIMovement")]
    public class AIMovementState : IdleMovementState
    {
        [SerializeField] protected Transform _path;

        private List<Transform> _points = new List<Transform>();

        private int _currentPointIndex;
        private Vector3 _defaultStartPoint;
        private float _timeSinceLastVisited;

        private bool _isOnPosition ;

        public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            base.OnEnter(characterStateBase, animator, stateInfo);

            _defaultStartPoint = animator.transform.position;
            _timeSinceLastVisited = 4;
            _isOnPosition = false;
            _distanceToAttack = _itemEquipper.GetAttackRange;

            if (_path != null)
            {
                foreach (Transform child in _path)
                {
                    _points.Add(child);
                }
            }
        }

        public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            AIInput(characterStateBase, animator, stateInfo);
        }

        private void AIInput(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (_attackRegistrator.AttackData.Target != null)
            {
                var targetPosition = _attackRegistrator.AttackData.Target.position;
                _movement.StartMoveTo(targetPosition, 1f);

                var distanceToTargetSec =
                    Vector3.Distance(animator.transform.position, targetPosition);
                
                if (distanceToTargetSec < _distanceToAttack)
                {
                    _movement.Cancel();
                    animator.transform.LookAt(_attackRegistrator.AttackData.Target.position);
                    animator.SetBool(Attack, true);
                }
               
                return;
            }

            if (Vector3Int.RoundToInt((_points[_currentPointIndex].position)) ==
                Vector3Int.RoundToInt((animator.transform.position)))
            {
                _isOnPosition = true;
                StayOnPlace(animator);
                animator.Play("LookAround");
                
                return;
            }
            
            if (_isOnPosition || _timeSinceLastVisited == 4)
            {
                _timeSinceLastVisited -= Time.deltaTime;

                if (_timeSinceLastVisited <= 0)
                {
                    _isOnPosition = false;
                    GoToNextWaypoint(animator);
                    _timeSinceLastVisited = 4;
                }
            }
            else
            {
                GoToNextWaypoint(animator);
            }
        }

        private void StayOnPlace(Animator animator)
        {
            if (_isOnPosition)
            {
                _currentPointIndex = (_currentPointIndex + 1) % _points.Count;
            }
        }

        private void GoToNextWaypoint(Animator animator)
        {
            if (_points.Count == 0)
            {
                _movement.StartMoveTo(_defaultStartPoint, 0.2f);
                return;
            }

            _movement.StartMoveTo(_points[_currentPointIndex].position, 0.2f);
        }
    }
}