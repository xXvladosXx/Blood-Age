using DialogueSystem;
using DialogueSystem.AIDialogue;
using Entity;
using InventorySystem;
using LootSystem;
using UnityEngine;

namespace StateMachine.PlayerStates
{
    public class PickingPlayerState : BasePlayerState
    {
        private ItemPicker _itemPicker;
        private ItemPickUp _lootObject;

        public override void GetComponents(AliveEntity aliveEntity)
        {
            base.GetComponents(aliveEntity);
            _itemPicker = aliveEntity.GetComponent<ItemPicker>();
        }
        public override void RunState(AliveEntity aliveEntity)
        {
            if (PointerOverUI()) return;
        }

        public override void StartState(AliveEntity aliveEntity)
        {
            
        }

        public override void EndState(AliveEntity aliveEntity)
        {
            
        }

        public override bool CanBeChanged => true;

        private void InterruptPicking()
        {
            StateSwitcher.SwitchState<IdlePlayerState>();
            _lootObject = null;
        }

        public void GoToPickUp(ItemPickUp lootObject)
        {
            _lootObject = lootObject;
            _itemPicker.AddItem(_lootObject, lootObject.Amount);
            InterruptPicking();
        }
    }
    
    
}