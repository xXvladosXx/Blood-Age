using System.Collections.Generic;
using System.Linq;
using StateMachine.EnemyStates;
using UnityEngine;

namespace EntitySpawnerSystem
{
    public class EnemyWave : MonoBehaviour
    {
        private List<EnemyStateManager> _enemy;
        public List<EnemyStateManager> Enemy => _enemy;

        private void Awake()
        {
            _enemy = GetComponentsInChildren<EnemyStateManager>().ToList();
            foreach (var manager in _enemy)
            {
                manager.gameObject.SetActive(false);
            }
        }
    }
}