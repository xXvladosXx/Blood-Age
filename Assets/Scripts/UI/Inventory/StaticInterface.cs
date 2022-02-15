using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory
{
    public class StaticInterface : UserInterface
    {
        [SerializeField] private GameObject[] _slots;
        protected override void CreateSlots()
        {
            SlotOnUI = new Dictionary<GameObject, Slot>();
            Index = 0;

            foreach (var inventorySlot in ItemContainer.Container.InventorySlots)
            {
                var o = _slots[Index];
                
                AddEvent(o, EventTriggerType.PointerEnter, delegate { OnEnter(o); });
                AddEvent(o, EventTriggerType.PointerExit, delegate { OnExit(); });
                AddEvent(o, EventTriggerType.BeginDrag, delegate { OnBeginDrag(o); });
                AddEvent(o, EventTriggerType.EndDrag, delegate { OnEndDrag(o); });
                AddEvent(o, EventTriggerType.Drag, delegate { OnDrag(); });
                
                SlotOnUI.Add(o, inventorySlot);
                Index++;
            }
        }

    }
}