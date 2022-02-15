using System;
using System.Collections.Generic;
using Entity;
using InventorySystem;
using TMPro;
using UI.Tooltip;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

namespace UI.Inventory
{
    public abstract class UserInterface : Panel
    {
        [SerializeField] protected ItemContainer ItemContainer;
        [SerializeField] protected bool ImageChanging = false;
        [SerializeField] protected bool HaveAccessToChangeOthers = true;
        public ItemContainer GetItemContainer => ItemContainer;

        protected int Index = 0;
        protected Dictionary<GameObject, Slot> SlotOnUI = new Dictionary<GameObject, Slot>();
        private MouseData _mouseData;
        
        public event Action OnItemDrag;
        public event Action OnItemPlace;

        public void SetMouseData(MouseData mouseData)
        {
            _mouseData = mouseData;
        }

        private void Awake()
        {
            CreateSlots();
        }

        public override void Initialize(AliveEntity aliveEntity)
        {
            AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
            AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        }

        private void Update()
        {
            if (ImageChanging)
                UpdateSlots();
            
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
            _mouseData.UI = o.GetComponent<UserInterface>();
        }

        protected void OnEnter(GameObject o)
        {
            var item = ItemContainer.FindNecessaryItemInData(SlotOnUI[o].ItemData.Id);
            
            ItemTooltipDistributor.Instance.ShowTooltip(item, this);
            SkillTooltip.Instance.ShowTooltip(item);

            _mouseData.TempItemHover = o;
        }

        protected void OnExit()
        {
            ItemTooltipDistributor.Instance.HideTooltip(this);
            SkillTooltip.Instance.HideTooltip();

            _mouseData.TempItemHover = null;
        }

        protected void OnBeginDrag(GameObject o)
        {
            OnItemDrag?.Invoke();
            ItemTooltipDistributor.Instance.HideTooltip(this);

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
                img.sprite = ItemContainer.FindNecessaryItemInData(SlotOnUI[o].ItemData.Id).UIDisplay;
                img.raycastTarget = false;
            }

            _mouseData.TempItemDrag = mouseObj;
            _mouseData.TempItemDrag.GetComponent<RectTransform>().SetAsLastSibling();
        }

        protected void OnDrag()
        {
            if (_mouseData.TempItemDrag == null) return;

            _mouseData.LastItemClicked = _mouseData.TempItemDrag;
            
            _mouseData.TempItemDrag.GetComponent<RectTransform>().position = Mouse.current.position.ReadValue();
            _mouseData.TempItemDrag.GetComponent<RectTransform>().SetAsLastSibling();
        }
        protected void OnEndDrag(GameObject o)
        {
            Destroy(_mouseData.TempItemDrag);
            OnItemPlace?.Invoke();

            if (!HaveAccessToChangeOthers && _mouseData.UI != this)
            {
                return;
            }

            if (_mouseData.UI == null)
            {
                SlotOnUI[o].RemoveItem();
                return;
            }

            if (_mouseData.TempItemHover && _mouseData.UI != this)
            {
                var mouseHoverSlot = _mouseData.UI.SlotOnUI[_mouseData.TempItemHover];

                ItemContainer.SwapItem(SlotOnUI[o], mouseHoverSlot);

                return;
            }

            if (_mouseData.TempItemHover)
            {
                var mouseHoverSlot = _mouseData.UI.SlotOnUI[_mouseData.TempItemHover];
                ItemContainer.SwapItem(SlotOnUI[o], mouseHoverSlot);
            }
        }

        private void OnDisable()
        {
            if (_mouseData.TempItemDrag != null)
            {
                Destroy(_mouseData.TempItemDrag);
                OnItemPlace?.Invoke();
            }
            
            ItemTooltipDistributor.Instance.HideTooltip(this);
            SkillTooltip.Instance.HideTooltip();
        }

        private void UpdateSlots()
        {
            foreach (var slot in SlotOnUI)
            {
                if (slot.Value.ItemData.Id >= 0)
                {
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                        ItemContainer.FindNecessaryItemInData(slot.Value.ItemData.Id).UIDisplay;
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