using DialogueSystem;
using DialogueSystem.AIDialogue;
using Entity;
using UnityEngine;

namespace StateMachine.PlayerStates
{
    public class DialoguePlayerState : BasePlayerState
    {
        private PlayerConversant _playerConversant;
        private AIConversant _aiConversant;

        public override void GetComponents(AliveEntity aliveEntity)
        {
            base.GetComponents(aliveEntity);
            _playerConversant = aliveEntity.GetComponent<PlayerConversant>();
            
        }

        public override void EndState(AliveEntity aliveEntity)
        {
            
        }

        public override bool CanBeChanged => true;

        
        public override void RunState(AliveEntity aliveEntity)
        {
         
        }

        public override void StartState(AliveEntity aliveEntity)
        {
            PlayerEntity.OnDied += entity => StateSwitcher.SwitchState<IdlePlayerState>();
            
        }

        private void EndDialogue()
        {
            if(_aiConversant != null)
                _aiConversant.DisableOutline();
            
            _playerConversant.Quit();
            StateSwitcher.SwitchState<IdlePlayerState>();
            _aiConversant = null;
        }

        public void StartDialogue(AIConversant aiConversant)
        {
            _playerConversant.OnDialogueEnd += EndDialogue;
            _aiConversant = aiConversant;

            Transform transform;
            (transform = _aiConversant.transform).LookAt(PlayerEntity.transform);
            PlayerEntity.transform.LookAt(transform);

            _playerConversant.StartDialogue(_aiConversant, aiConversant.GetDialogue);
           
        }
    }
}