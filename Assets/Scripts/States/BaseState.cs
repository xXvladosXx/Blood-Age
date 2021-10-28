using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : StateMachineBehaviour
{
    public List<StateData> ListAbilityData = new List<StateData>();
    private Vector2 _movementInput;
    private Vector2 _cameraInput;
    public void UpdateAll(BaseState characterStateBase, Animator animator, AnimatorStateInfo animatorStateInfo)
    {
        foreach (var stateData in ListAbilityData)
        {
            stateData.UpdateAbility(characterStateBase, animator, animatorStateInfo);
        }
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var stateData in ListAbilityData)
        {
            stateData.OnEnter(this, animator, stateInfo);
        }
    }
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        UpdateAll(this, animator, animatorStateInfo);
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var stateData in ListAbilityData)
        {
            stateData.OnExit(this, animator, stateInfo);
        }
    }
   
}
