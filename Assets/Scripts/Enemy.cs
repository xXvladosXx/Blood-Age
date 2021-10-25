using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.MouseSystem;
using UnityEngine;

public class Enemy : MonoBehaviour, IRaycastable
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public bool HandleRaycast(Transform gameObject)
    {
        return false;
    }

    public void TakeHit()
    {
        _animator.Play("Hit");
    }
}
