using DialogueSystem;
using DialogueSystem.AIDialogue;
using Entity;
using MouseSystem;
using UI.Shop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

        public override void RunState(AliveEntity aliveEntity)
        {
            if (PointerOverUI()) return;
            if (StarterAssetsInputs.ButtonInput)
            {
                RaycastHit raycastHit;
                Physics.Raycast(AliveEntity.GetRay(), out raycastHit, Mathf.Infinity);
                if (raycastHit.collider.TryGetComponent(out AIConversant aiConversant))
                {
                    if (aiConversant == _aiConversant) return;
                    
                }
                
                EndDialogue();
            }
        }

        public override void StartState(float time)
        {
            
        }

        public void EndDialogue()
        {
            _aiConversant.DisableOutline();
            _playerConversant.Quit();
            StateSwitcher.SwitchState<IdlePlayerState>();
            _aiConversant = null;
        }

        public void StartDialogue(AIConversant aiConversant)
        {
            _aiConversant = aiConversant;
            _aiConversant.EnableOutLine();
            _playerConversant.StartDialogue(_aiConversant, aiConversant.GetDialogue);
        }
    }
}