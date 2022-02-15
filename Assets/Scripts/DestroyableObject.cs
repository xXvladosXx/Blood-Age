using System;
using DefaultNamespace;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private float _time;
    private void Start()
    {
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject, _time);
    }
}