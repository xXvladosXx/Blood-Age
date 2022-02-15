using Entity;
using UnityEngine;

namespace StateMachine.EnemyStates
{
    [CreateAssetMenu(fileName = "DistanceConfiguration", menuName = "Configuration/DistanceConfiguration")]
    public class StateDistanceConfiguration : ScriptableObject
    {
        public float ChaseDistance = 15f;
        public float DamageChaseDistance = 30f;
        
        public bool IsInRange(AliveEntity target, AliveEntity user, float range)
        {
            if (target == null || target.GetHealth.IsDead()) return false;
        
            return Vector3.Distance( target.transform.position, user.transform.position) < range;
        }
    }
}