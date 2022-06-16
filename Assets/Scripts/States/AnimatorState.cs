using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace States
{
    public class AnimatorState : StateMachineBehaviour
    {
        public List<StateData> ListAbilityData = new List<StateData>();
        private PlayerInputs _playerInputs;

        private void UpdateAll(AnimatorState characterStateAnimator, Animator animator, AnimatorStateInfo animatorStateInfo)
        {
            foreach (var stateData in ListAbilityData)
            {
                stateData.UpdateAbility(characterStateAnimator, animator, animatorStateInfo);
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerInputs = animator.GetComponent<PlayerInputs>();
            if(_playerInputs == null) return;

            foreach (var stateData in ListAbilityData)
            {
                stateData.OnEnter(this, animator, stateInfo);
            }
        }
    
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if(_playerInputs == null) return;

            UpdateAll(this, animator, animatorStateInfo);
        }
    
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_playerInputs == null) return;

            foreach (var stateData in ListAbilityData)
            {
                stateData.OnExit(this, animator, stateInfo);
            }
        }
   
    }
}
