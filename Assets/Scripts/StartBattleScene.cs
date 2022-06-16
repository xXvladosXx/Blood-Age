using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattleScene : MonoBehaviour
{
    [SerializeField] private List<Animator> _animators;
    private static readonly int Battle = Animator.StringToHash("Battle");

    private void Awake()
    {
        
    }

    private void StartBattle()
    {
        foreach (var animator in _animators)
        {
            animator.SetBool(Battle, true);
        }
    }
}
