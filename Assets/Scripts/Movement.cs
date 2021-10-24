using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private float _maxSpeed;
    private NavMeshAgent _navMeshAgent;
        
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _maxSpeed = _navMeshAgent.speed;
    }

    public void StartMoveTo(Vector3 destination, float speedFraction)
    {
        if(_navMeshAgent.enabled == false) return;
            
        _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
        _navMeshAgent.destination = destination;
        _navMeshAgent.isStopped = false;    
    }
}