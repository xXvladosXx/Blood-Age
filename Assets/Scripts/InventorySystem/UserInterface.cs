using InventorySystem;
using UnityEngine;

namespace InventorySystem
{
    using System;
    using System.Collections.Generic;
    using DefaultNamespace.Entity;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.InputSystem;
    using UnityEngine.PlayerLoop;
    using UnityEngine.UI;


    public abstract class UserInterface : MonoBehaviour
    {
        [SerializeField] protected Inventory inventory;
        public Inventory GetInventory => inventory;
        
        protected int Index = 0;
        protected Dictionary<GameObject, InventorySlot> SlotOnUI = new Dictionary<GameObject, InventorySlot>();
        private MouseData _mouseData;

        public event Action OnItemDrag;
        public event Action OnItemPlace;
        public event Action<Item> OnItemEquip;
        public event Action<Item> OnItemUnequip;

        public void SetMouseData(MouseData mouseData)
        {
            _mouseData = mouseData;
        }

        private void Awake()
        {
            CreateSlots();
        }

        private void Start()
        {
            AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
            AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        }

        protected abstract void CreateSlots();

        protected void AddEvent(GameObject o, EventTriggerType pointerAction, UnityAction<BaseEventData> action)
        {
            var trigger = o.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry() { eventID = pointerAction };

            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        private void OnExitInterface(GameObject o)
        {
           _mouseData.UI = null;
        }

        private void OnEnterInterface(GameObject o)
        {
            OnItemDrag?.Invoke();
            _mouseData.UI = o.GetComponent<UserInterface>();
        }

        protected void OnEnter(GameObject o)
        {
            _mouseData.TempItemHover = o;
        }

        protected void OnExit()
        {
            _mouseData.TempItemHover = null;
        }

        protected void OnBeginDrag(GameObject o)
        {
            var mouseObj = new GameObject
            {
                transform =
                {
                    parent = gameObject.transform.parent
                }
            };

            var rt = mouseObj.AddComponent<RectTransform>();

            rt.sizeDelta = new Vector2(50, 50);

            if (SlotOnUI[o].ItemData.Id >= 0)
            {
                var img = mouseObj.AddComponent<Image>();
                img.sprite = inventory.FindNecessaryItem(SlotOnUI[o].ItemData.Id).UIDisplay;
                img.raycastTarget = false;
            }

            _mouseData.TempItemDrag = mouseObj;
            _mouseData.TempItemDrag.GetComponent<RectTransform>().SetAsLastSibling();
        }

        protected void OnDrag()
        {
            if (_mouseData.TempItemDrag == null) return;

            _mouseData.TempItemDrag.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
            _mouseData.TempItemDrag.GetComponent<RectTransform>().SetAsLastSibling();
        }

        protected void OnEndDrag(GameObject o)
        {
            Destroy(_mouseData.TempItemDrag);

            if (_mouseData.UI == null)
            {
                SlotOnUI[o].RemoveItem();
                return;
            }

            if (_mouseData.TempItemHover && _mouseData.UI != this)
            {
                var mouseHoverSlot = _mouseData.UI.SlotOnUI[_mouseData.TempItemHover];
                inventory.SwapItem(SlotOnUI[o], mouseHoverSlot);
                
                if (mouseHoverSlot.ItemData.Id >= 0 && _mouseData.UI is StaticInterface)
                {
                    var itemObject = inventory.FindNecessaryItem(mouseHoverSlot.ItemData.Id);

                    OnItemEquip?.Invoke(itemObject);
                    
                }
                else
                {
                    var itemObject = inventory.FindNecessaryItem(mouseHoverSlot.ItemData.Id);

                    OnItemUnequip?.Invoke(itemObject);
                }

                return;
            }

            if (_mouseData.TempItemHover)
            {
                var mouseHoverSlot = _mouseData.UI.SlotOnUI[_mouseData.TempItemHover];
                inventory.SwapItem(SlotOnUI[o], mouseHoverSlot);
            }
        }

        private void Update()
        {
            UpdateSlots();
        }

        private void UpdateSlots()
        {
            foreach (var slot in SlotOnUI)
            {
                if (slot.Value.ItemData.Id >= 0)
                {
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                        inventory.FindNecessaryItem(slot.Value.ItemData.Id).UIDisplay;
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                    slot.Key.GetComponentInChildren<TextMeshProUGUI>().text =
                        slot.Value.Amount == 1 || slot.Value.Amount == 0 ? string.Empty : slot.Value.Amount.ToString();
                }
                else
                {
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                    slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
                }
            }
        }
    }
}