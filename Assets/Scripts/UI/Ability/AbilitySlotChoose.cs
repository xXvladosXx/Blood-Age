using System.Collections.Generic;
using InventorySystem;
using SkillSystem.Skills;

namespace UI.Ability
{
    public class AbilitySlotChoose : SlotChoose
    {
        public override void SetData(ItemContainer itemContainer, Slot slotToChange, SlotHandler slotHandler)
        {
            _index = 0;
            base.itemContainer = itemContainer;
            foreach (var key in SlotOnUI.Keys)
            {
                Destroy(key.gameObject);
            }
            SlotOnUI.Clear();
            
            foreach (var inventorySlot in base.itemContainer.Container.InventorySlots)
            {
                var o = Instantiate(itemPrefab, transform);
                o.OnAbilityClick += OpenAbilityChoose;
                
                SlotOnUI.Add(o, inventorySlot);
                _index++;
            }

            SlotToChange = slotToChange;
            SlotHandler = slotHandler;
            UpdateSlots();
        }

    }
}