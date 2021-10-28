using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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

        _wasClickedOnTarget = false;
        _wasSpawned = false;
    }

    public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        Debug.Log(_wasClickedOnTarget);
        
        base.UpdateAbility(characterStateBase, animator, stateInfo);
        CombatCheck(characterStateBase, animator, stateInfo);
        CheckDistance();
        ActivateIndicatorOnTarget(stateInfo);
        DeactivateIndicatorOnTarget(stateInfo);

        if (_starterAssetsInputs.ButtonInput)
        {
            _wasClickedOnTarget = true;
        }
    }

    private void DeactivateIndicatorOnTarget(AnimatorStateInfo stateInfo)
    {
        if (_attackRegistrator.AttackData.Target == null) return;

        if (stateInfo.normalizedTime > _endAttackTime)
        {
            if (_wasSpawned)
            {
                _attackRegistrator.AttackData.Target.GetComponentInChildren<IndicatorActivator>().DeactivateIndicator();
            }
        }
    }

    private void ActivateIndicatorOnTarget(AnimatorStateInfo stateInfo)
    {
        if (_wasSpawned) return;
        if (!(stateInfo.normalizedTime >= _startAttackTime + ((_endAttackTime - _startAttackTime)) / _timeModifier)) return;
        if (_attackRegistrator.AttackData.Target != null)
        {
            _wasSpawned = true;
            _attackRegistrator.AttackData.Target.GetComponentInChildren<IndicatorActivator>()
                .ActivateIndicator(_effect);
        }
    }

    private void CombatCheck(BaseState characterStateBase, Animator animator,
        AnimatorStateInfo stateInfo)
    {
        RaycastHit raycastHit;
        Physics.Raycast(_starterAssetsInputs.GetRay(), out raycastHit);

        switch (_starterAssetsInputs.ButtonInput)
        {
            case true when raycastHit.collider.GetComponent<Enemy>() != null:
            {
                _attackRegistrator.AttackData.Target = raycastHit.collider.transform;
                
                if (!(stateInfo.normalizedTime >= _startAttackTime + ((_endAttackTime - _startAttackTime)) / _timeModifier)) return;
                if (_wasClickedOnTarget) return;

                animator.SetBool(WasRegistered, true);
                _wasClickedOnTarget = false;

                break;
            }
            case true when raycastHit.collider.GetComponent<Enemy>() == null:
            {
                if(_attackRegistrator.AttackData.Target != null)
                    _attackRegistrator.AttackData.Target.GetComponentInChildren<IndicatorActivator>().DeactivateIndicator();

                animator.SetBool(ForceTransition, true);
                animator.SetBool(MainAttack, false);

                _wasClickedOnTarget = false;

                _movement.StartMoveTo(raycastHit.point, 1f);
                _attackRegistrator.AttackData.Target = null;
                break;
            }
        }
    }

    private void CheckDistance()
    {
        if (_attackRegistrator.AttackData.Target == null) return;
        
        float distanceTo = Vector3.Distance(_attackRegistrator.transform.position,
            _attackRegistrator.AttackData.Target.position);

        if (distanceTo > _distanceToAttack)
            _movement.StartMoveTo(_attackRegistrator.AttackData.Target.position, 1f);
    }

    public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        _wasClickedOnTarget = false;
        _wasSpawned = false;
        _attackRegistrator.DisableCollider();
    }
}