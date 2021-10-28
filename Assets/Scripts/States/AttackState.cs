using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackState : StateData
{
    [SerializeField] protected float _startAttackTime;
    [SerializeField] protected float _endAttackTime;
    [SerializeField] protected float _distanceToAttack;
    
    protected AttackRegistrator _attackRegistrator;
    protected Movement _movement;

    protected bool _wasClickedOnTarget;
    protected static readonly int MainAttack = Animator.StringToHash("Attack");
    protected static readonly int WasRegistered = Animator.StringToHash("WasRegistered");
    protected static readonly int ForceTransition = Animator.StringToHash("ForceTransition");

    public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        _movement = animator.GetComponent<Movement>();
        _attackRegistrator = animator.GetComponentInChildren<AttackRegistrator>();

        _movement.Cancel();
    }

    public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        RegisterAttack(characterStateBase, animator, stateInfo);
        DeregisterAttack(characterStateBase, stateInfo, animator);
    }

    private void RegisterAttack(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (!(_startAttackTime <= stateInfo.normalizedTime) || !(_endAttackTime > stateInfo.normalizedTime)) return;
       
        _attackRegistrator.EnableCollider();
    }

    private void DeregisterAttack(BaseState characterStateBase, AnimatorStateInfo stateInfo, Animator animator)
    {
        if (!(_endAttackTime <= stateInfo.normalizedTime)) return;

        _attackRegistrator.DisableCollider();
        
        animator.SetBool(WasRegistered, false);
        animator.SetBool(MainAttack, false);
    }

    

    public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        _attackRegistrator.DisableCollider();
    }
}