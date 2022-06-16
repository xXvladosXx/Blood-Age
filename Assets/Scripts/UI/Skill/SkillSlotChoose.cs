using System.Collections.Generic;
using InventorySystem;
using SkillSystem.Skills;
using UI.Ability;

namespace UI.Skill
{
    public class SkillSlotChoose : SlotChoose
    {
        public override void SetData(ItemContainer actionItems, ItemContainer knownItems, Slot slotToChange,
            SlotHandler slotHandler)
        {
            Index = 0;
            PlayerContainer = actionItems;
            ItemContainer = knownItems;
            
            foreach (var key in SlotOnUI.Keys)
            {
                Destroy(key.gameObject);
            }
            
            SlotOnUI.Clear();
            
            SlotOnUI = new Dictionary<SlotHandler, Slot>();

            foreach (var inventorySlot in knownItems.Container.InventorySlots)
            {
                if (!(knownItems.Database.GetItemByID(inventorySlot.ItemData.Id) is ActiveSkill)) continue;

                var o = Instantiate(ItemPrefab, transform);
                o.OnAbilityClick += OpenAbilityChoose;
                
                SlotOnUI.Add(o, inventorySlot);
                Index++;
            }

            SlotToChange = slotToChange;
            SlotHandler = slotHandler;
            UpdateSlots();
        }
    }
}