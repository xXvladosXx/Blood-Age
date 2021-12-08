using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Entity;
using DefaultNamespace.MouseSystem;
using InventorySystem;
using UnityEngine;
using UnityEngine.AI;

public abstract class AttackState : StateData
{
    [SerializeField] protected float startAttackTime;
    [SerializeField] protected float endAttackTime;
    [SerializeField] protected bool heavyAttack;
    
    protected float DistanceToAttack;
    protected AttackRegistrator AttackRegistrator;
    protected AliveEntity AliveEntity;

    protected bool WasClickedOnTarget;
    protected static readonly int MainAttack = Animator.StringToHash("Attack");
    protected static readonly int WasRegistered = Animator.StringToHash("WasRegistered");
    protected static readonly int ForceTransition = Animator.StringToHash("ForceTransition");

    public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        AttackRegistrator = animator.GetComponentInChildren<AttackRegistrator>();
        AttackRegistrator.AttackData.HeavyAttack = heavyAttack;
        
        animator.SetBool(WasRegistered, false);
        animator.SetBool(MainAttack, false);
        
        AliveEntity = animator.GetComponent<AliveEntity>();
        
        DistanceToAttack = AliveEntity.GetItemEquipper.GetAttackRange;
    }

    public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        RegisterAttack(characterStateBase, animator, stateInfo);
        DeregisterAttack(characterStateBase, stateInfo, animator);
    }

    private void RegisterAttack(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (!(startAttackTime <= stateInfo.normalizedTime) || !(endAttackTime > stateInfo.normalizedTime)) return;
        
        AttackRegistrator.EnableCollider();
    }

    private void DeregisterAttack(BaseState characterStateBase, AnimatorStateInfo stateInfo, Animator animator)
    {
        if (!(endAttackTime <= stateInfo.normalizedTime)) return;

        AttackRegistrator.DisableCollider();
        
        animator.SetBool(WasRegistered, false);
        animator.SetBool(MainAttack, false);
    }

    

    public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        animator.SetBool(WasRegistered, false);
        animator.SetBool(MainAttack, false);
        AttackRegistrator.DisableCollider();
    }
}