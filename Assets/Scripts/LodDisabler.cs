using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodDisabler : MonoBehaviour
{
    private void Awake()
    {
        foreach (var lodGroup in GetComponentsInChildren<LODGroup>())
        {
            lodGroup.enabled = false;
        }

        foreach (var collider in GetComponentsInChildren<MeshCollider>())
        {
            Destroy(collider);
        }
        
        foreach (var collider in GetComponentsInChildren<CapsuleCollider>())
        {
            Destroy(collider);
        }
    }
}
