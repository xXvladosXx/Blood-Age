using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ColliderDisabler : MonoBehaviour
    {
        private void Awake()
        {
            foreach (var child in GetComponentsInChildren<Collider>())
            {
                child.enabled = false;
            }
        }
    }
}