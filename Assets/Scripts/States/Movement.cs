using System;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
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
        
        _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
        _navMeshAgent.destination = destination;
        _navMeshAgent.isStopped = false;    
    }

    public void Cancel()
    {
        _navMeshAgent.ResetPath();
        _navMeshAgent.isStopped = true;    
    }
}