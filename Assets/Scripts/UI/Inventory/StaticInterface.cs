namespace InventorySystem
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class StaticInterface : UserInterface
    {
        [SerializeField] private GameObject[] _slots;
        protected override void CreateSlots()
        {
            SlotOnUI = new Dictionary<GameObject, InventorySlot>();
            Index = 0;

            foreach (var inventorySlot in inventory.InventoryContainer.InventorySlots)
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