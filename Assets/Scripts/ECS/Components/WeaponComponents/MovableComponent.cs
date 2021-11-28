using Leopotam.Ecs;
using UnityEngine.AI;

namespace Movable
{
    using System;

    [Serializable]
    public struct MovableComponent
    {
        public NavMeshAgent navMeshAgent;
        public float speed;
    }
}
    