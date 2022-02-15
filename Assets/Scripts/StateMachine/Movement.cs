using SaveSystem;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachine
{
    public class Movement : MonoBehaviour, ISavable
    {
        private float _maxSpeed;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _maxSpeed = _navMeshAgent.speed; 
        }

        private void Update()
        {
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
        
            _animator.SetFloat(Speed, speed);
        }
    
        public void StartMoveTo(Vector3 destination, float speedFraction)
        {
            if(!_navMeshAgent.enabled) return;
            print("MNoibe");

            _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;    
        }
        public void StartMoveTo(Vector3 destination, float speedFraction, float speed)
        {
            if(!_navMeshAgent.enabled) return;
        
            _navMeshAgent.speed = speed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;    
        }
        public void Cancel()
        {
            if (_navMeshAgent.enabled)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.isStopped = true;
            }
        }

        public void EnableMovement(bool enable)
        {
            _navMeshAgent.enabled = enable;
        }
        
        public object CaptureState()
        {
            return new SerializableVector(transform.position);
        }

        public void RestoreState(object state)
        {
            print("restored");
            SerializableVector position =(SerializableVector)state;

            _navMeshAgent.enabled = false;
            transform.position = position.ToVector();
            _navMeshAgent.enabled = true;
        
            Cancel();
        }
    }
}