using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Entity;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Attack", menuName = "State/AttackState")]
public class PlayerAttackState : AttackState
{
    [SerializeField] private GameObject _effect;
    [SerializeField] private int _timeModifier = 100;
    
    private StarterAssetsInputs _starterAssetsInputs;
    private bool _wasSpawned;
    public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        base.OnEnter(characterStateBase, animator, stateInfo);
        _starterAssetsInputs = animator.GetComponent<StarterAssetsInputs>();

        WasClickedOnTarget = false;
    }

    public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (!AliveEntity.GetItemEquipper.IsRanged)
        {
            base.UpdateAbility(characterStateBase, animator, stateInfo);
        }
        
        animator.transform.LookAt(AttackRegistrator.AttackData.Target);
        CombatCheck(characterStateBase, animator, stateInfo);
        
    }


    private void CombatCheck(BaseState characterStateBase, Animator animator,
        AnimatorStateInfo stateInfo)
    {
        RaycastHit raycastHit;
        Physics.Raycast(_starterAssetsInputs.GetRay(), out raycastHit);

        switch (_starterAssetsInputs.ButtonInput)
        {
            case true when raycastHit.collider.GetComponent<AliveEntity>() != null
                           && raycastHit.collider.GetComponent<AliveEntity>() != animator.GetComponent<AliveEntity>():
            {
                WasClickedOnTarget = true;
                AttackRegistrator.AttackData.Target = raycastHit.collider.transform;

                if (stateInfo.normalizedTime >= startAttackTime )
                {
                    animator.SetBool(WasRegistered, true);
                }

                break;
            }
            case true when raycastHit.collider.GetComponent<AliveEntity>() == null:
            {
                AttackRegistrator.AttackData.Target = null;    
                animator.SetBool(ForceTransition, true);
                animator.SetBool(MainAttack, false);
                break;
            }
        }
    }

}