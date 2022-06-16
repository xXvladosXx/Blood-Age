using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI.Inventory
{
    public class DynamicInterface : UserInterface
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Transform _content;

        protected override void CreateSlots()
        {
            SlotOnUI = new Dictionary<GameObject, Slot>();

            foreach (var inventorySlot in ItemContainer.Container.InventorySlots)
            {
                var o = Instantiate(_itemPrefab, _content);
                
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