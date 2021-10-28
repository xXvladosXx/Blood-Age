using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateData : ScriptableObject
{
    public abstract void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo);
    public abstract void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo);
    public abstract void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo);
}