using DialogueSystem;
using DialogueSystem.AIDialogue;
using Entity;
using ShopSystem;
using StateMachine.BaseStates;
using UnityEngine;

namespace StateMachine.PlayerStates
{
    public class ShopPlayerState : BasePlayerState
    {
        private Seller _seller;
        private bool _closed;
        private Customer _customer;
        private AIConversant _aiConversant;

        public override void RunState(AliveEntity aliveEntity)
        {
        }

        public override void StartState(AliveEntity aliveEntity)
        {
           
        }

        public override void EndState(AliveEntity aliveEntity)
        {
            
        }

        public override bool CanBeChanged => true;

        public void StartShopping(AIConversant aiConversant, PlayerConversant customer)
        {
            _aiConversant = aiConversant;
            _seller = aiConversant.GetComponent<Seller>();
            _customer = customer.GetComponent<Customer>();
            
            _seller.OnShopClose += () => StateSwitcher.SwitchState<IdlePlayerState>();
            
            _closed = false;
        }
    }
}