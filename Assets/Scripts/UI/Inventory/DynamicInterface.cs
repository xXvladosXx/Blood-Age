namespace InventorySystem
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class DynamicInterface : UserInterface
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private int xStart = -100;
        [SerializeField] private int yStart = 90;
        [SerializeField] private int columns = 8;
        [SerializeField] private int spaceY = 50;
        [SerializeField] private int spaceX = 30;
        protected override void CreateSlots()
        {
            SlotOnUI = new Dictionary<GameObject, InventorySlot>();

            foreach (var inventorySlot in inventory.InventoryContainer.InventorySlots)
            {
                var o = Instantiate(_itemPrefab, transform);
                o.GetComponent<RectTransform>().localPosition = GetPosition(Index);
                
                AddEvent(o, EventTriggerType.PointerEnter, delegate { OnEnter(o); });
                AddEvent(o, EventTriggerType.PointerExit, delegate { OnExit(); });
                AddEvent(o, EventTriggerType.BeginDrag, delegate { OnBeginDrag(o); });
                AddEvent(o, EventTriggerType.EndDrag, delegate { OnEndDrag(o); });
                AddEvent(o, EventTriggerType.Drag, delegate { OnDrag(); });
            
                SlotOnUI.Add(o, inventorySlot);
                Index++;
            }
        }

        private Vector2 GetPosition(int index)
        {
            return new Vector2(xStart + (spaceX * (index * 2 % columns)), yStart + (-spaceY * (index * 2 / columns)));
        }
    }
}