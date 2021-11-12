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
    private IndicatorActivator _targetIndicator;
    private bool _wasSpawned;
    public override void OnEnter(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        base.OnEnter(characterStateBase, animator, stateInfo);
        _starterAssetsInputs = animator.GetComponent<StarterAssetsInputs>();

        _wasClickedOnTarget = false;
        _wasSpawned = false;
        
        if(_attackRegistrator.AttackData.Target != null)
            _targetIndicator = _attackRegistrator.AttackData.Target.GetComponentInChildren<IndicatorActivator>();
    }

    public override void UpdateAbility(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (!_itemEquipper.IsRanged)
            base.UpdateAbility(characterStateBase, animator, stateInfo);
        
        animator.transform.LookAt(_attackRegistrator.AttackData.Target);
        CombatCheck(characterStateBase, animator, stateInfo);
        
        if(_targetIndicator == null) return;
        ActivateIndicatorOnTarget(stateInfo);
        DeactivateIndicatorOnTarget(stateInfo);
    }

    private void DeactivateIndicatorOnTarget(AnimatorStateInfo stateInfo)
    {
        if (!(stateInfo.normalizedTime > _endAttackTime)) return;

        _targetIndicator.DeactivateIndicator();
        _wasSpawned = false;
    }
    
    private void DeactivateIndicatorOnTarget()
    {        
        _targetIndicator.DeactivateIndicator();
        _wasSpawned = false;
    }

    private void ActivateIndicatorOnTarget(AnimatorStateInfo stateInfo)
    {
        if (_wasSpawned) return;
        if (!(stateInfo.normalizedTime >= _startAttackTime + ((_endAttackTime - _startAttackTime)) / _timeModifier)) return;

        _wasSpawned = true;
        _targetIndicator.ActivateIndicator(_effect);
    }

    private void CombatCheck(BaseState characterStateBase, Animator animator,
        AnimatorStateInfo stateInfo)
    {
        RaycastHit raycastHit;
        Physics.Raycast(_starterAssetsInputs.GetRay(), out raycastHit);

        switch (_starterAssetsInputs.ButtonInput)
        {
            case true when raycastHit.collider.GetComponent<Health>() != null && raycastHit.collider.GetComponent<Health>() != animator.GetComponent<Health>():
            {
                _wasClickedOnTarget = true;
                _attackRegistrator.AttackData.Target = raycastHit.collider.transform;
                _targetIndicator = _attackRegistrator.AttackData.Target.GetComponentInChildren<IndicatorActivator>();
                
                if(!_wasSpawned) return;
                if (!_wasClickedOnTarget) return;

                animator.SetBool(WasRegistered, true);
                _wasClickedOnTarget = false;

                break;
            }
            case true when raycastHit.collider.GetComponent<Health>() == null:
            {
                DeactivateIndicatorOnTarget();

                _attackRegistrator.AttackData.Target = null;    
                animator.SetBool(ForceTransition, true);
                animator.SetBool(MainAttack, false);
                break;
            }
        }
    }

    public override void OnExit(BaseState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        base.OnExit(characterStateBase, animator, stateInfo);
        
        DeactivateIndicatorOnTarget();
    }
}