using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Attack", menuName = "State/AttackState")]
public class AttackState : StateData
{
    [SerializeField] private float _startAttackTime;
    [SerializeField] private float _endAttackTime;

    private AttackMaker _attackMaker;
    private StarterAssetsInputs _starterAssetsInputs;
    private Movement _movement;

    private bool _wasRegistered;
    private static readonly int MainAttack = Animator.StringToHash("Attack");
    private static readonly int WasRegistered = Animator.StringToHash("WasRegistered");
    private static readonly int ForceTransition = Animator.StringToHash("ForceTransition");

    public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        _movement = animator.GetComponent<Movement>();
        _attackMaker = animator.GetComponentInChildren<AttackMaker>();
        _starterAssetsInputs = animator.GetComponent<StarterAssetsInputs>();

        _movement.Cancel();
    }

    public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        RegisterAttack(characterStateBase, animator, stateInfo);
        DeregisterAttack(characterStateBase, stateInfo, animator);
        CombatCheck(characterStateBase, animator, stateInfo);
    }

    private void RegisterAttack(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (!(_startAttackTime <= stateInfo.normalizedTime) || !(_endAttackTime > stateInfo.normalizedTime)) return;
        
        _attackMaker.EnableCollider();
    }

    private void DeregisterAttack(BaseState characterStateBase, AnimatorStateInfo stateInfo, Animator animator)
    {
        if (!(_endAttackTime <= stateInfo.normalizedTime)) return;

        _attackMaker.DisableCollider();
        _wasRegistered = false;
        animator.SetBool(WasRegistered, false);
        animator.SetBool(MainAttack, false);
    }

    private void CombatCheck(BaseState characterStateBase, Animator animator,
        AnimatorStateInfo stateInfo)
    {
        RaycastHit raycastHit;
        Physics.Raycast(_starterAssetsInputs.GetRay(), out raycastHit);

        switch (_starterAssetsInputs.ButtonInput)
        {
            case true when raycastHit.transform.GetComponent<Enemy>() == null:
            {
                _attackMaker.AttackData.Target = null;
                if (raycastHit.collider.GetComponent<Enemy>() != null)
                {
                    _attackMaker.AttackData.Target = raycastHit.transform;
                }

                animator.SetBool(ForceTransition, true);
                animator.SetBool(MainAttack, false);

                _movement.StartMoveTo(raycastHit.point, 1f);
                break;
            }
            case true when raycastHit.transform.GetComponent<Enemy>() != null:
            {
                if (stateInfo.normalizedTime >= _startAttackTime + ((_endAttackTime - _startAttackTime)) / 3f)
                {
                    if (stateInfo.normalizedTime < _endAttackTime)
                    {
                        animator.SetBool(WasRegistered, true);
                    }
                }

                break;
            }
        }
    }

    public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
    }
}