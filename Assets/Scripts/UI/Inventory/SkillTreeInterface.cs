using System.Linq;
using UI.Skill;

namespace UI.Inventory
{
    using System.Collections.Generic;
    using InventorySystem;
    using SkillSystem;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class SkillTreeInterface : UserInterface
    {
        [SerializeField] private GameObject[] _slots;

        protected override void CreateSlots()
        {
            SlotOnUI = new Dictionary<GameObject, Slot>();
            Index = 0;

            foreach (var inventorySlot in ItemContainer.Container.InventorySlots)
            {
                if(Index >= _slots.Length) return;
                
                var o = _slots[Index];
                
                AddEvent(o, EventTriggerType.PointerEnter, delegate { OnEnter(o); });
                AddEvent(o, EventTriggerType.PointerExit, delegate { OnExit(); });
               
                SlotOnUI.Add(o, inventorySlot);

                Index++;
            }
        }
    }
}