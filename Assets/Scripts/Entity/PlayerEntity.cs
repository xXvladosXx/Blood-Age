using System;
using System.Collections.Generic;
using InventorySystem;
using InventorySystem.Items;
using MouseSystem;
using SaveSystem;
using ShopSystem;
using UI.Stats;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Entity
{
    [SerializeField]
    public class PlayerScene
    {
        public bool CanSave;
    }
    
    public class PlayerEntity : AliveEntity, ISavable
    {
        [SerializeField] private ItemContainer _hotbarItems;

        private Camera _mainCamera;
        private ItemPicker _itemPicker;
        private Customer _customer;
        private bool _canSave;

        [Serializable]
        struct CursorIterating
        {
            public CursorType Cursor;
            public Vector2 Hotspot;
            public Texture2D Texture2D;
        }

        [SerializeField] private CursorIterating[] _cursorIteratings;

        
        protected override void Update()
        {
            base.Update();
            if (Keyboard.current.zKey.wasPressedThisFrame)
            {
                UseItem(0, this);
            }
            if (Keyboard.current.xKey.wasPressedThisFrame)
            {
                UseItem(1, this);
            }
            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                UseItem(2, this);
            }
            
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                DisableAllRaycastBars();
            }
			
            if(PointerOverUI()) return;
            if(InteractWithComponent()) return;
            if(OrdinaryCursor()) return;
        }
        protected override void Init()
        {
            _mainCamera = Camera.main;
            _itemPicker = GetComponent<ItemPicker>();
            _customer = GetComponent<Customer>();

            _customer.OnItemBuy += CheckAddingToHotBar;
            _itemPicker.OnItemPickUp += CheckAddingToHotBar;
            
        }

        private void CheckAddingToHotBar(InventoryItem obj)
        {
            if (_hotbarItems.HasItemInInventory(obj.Data))
            {
                _hotbarItems.AddItem(obj.Data, _itemPicker.GetItemContainer.FindSlotInInventory(obj.Data).Amount);
                _itemPicker.GetItemContainer.RemoveItem(obj.Data.Id, _itemPicker.GetItemContainer.FindSlotInInventory(obj.Data).Amount);
            }
        }

        private void UseItem(int index, AliveEntity aliveEntity)
        {
            if(_hotbarItems.GetInventoryItems().Count <= index) return;
            if (_hotbarItems.GetInventoryItems()[index] is IConsumable consumable)
            {
                _hotbarItems.RemoveItem(_hotbarItems.GetInventoryItems()[index].Data.Id, 1);
                consumable.Consume(aliveEntity);
            }
        }

        private bool OrdinaryCursor()
        {
            RaycastHit raycastHit;
            bool hasHit = Physics.Raycast(GetRay(), out raycastHit);

            if (hasHit)
            {
                SetCursor(CursorType.Movement);
                return true;
            }

            SetCursor(CursorType.None);
            return false;
        }

        private bool PointerOverUI()
        {
            if (!EventSystem.current.IsPointerOverGameObject()) return false;

            SetCursor(CursorType.UI);
            return true;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetRay());

            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (!raycastable.HandleRaycast(this)) continue;
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        raycastable.ClickAction();
                    }

                    SetCursor(raycastable.GetCursorType());
                    return true;
                }
            }

            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorIterating iterating = GetCursorIterating(type);
            Cursor.SetCursor(iterating.Texture2D, iterating.Hotspot, CursorMode.Auto);
        }

        private CursorIterating GetCursorIterating(CursorType type)
        {
            foreach (var cursorIterating in _cursorIteratings)
            {
                if (cursorIterating.Cursor == type)
                    return cursorIterating;
            }

            return _cursorIteratings[0];
        }


        private void DisableAllRaycastBars()
        {
            HealthBarEntity.Instance.HideHealth();
        }
        
        public Ray GetRay()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            return ray;
        }

        public object CaptureState()
        {
            var items = _hotbarItems.GetAllSlots();
            return items;
        }

        public void RestoreState(object state)
        {
            var items = state as List<Slot>;
            _hotbarItems.ClearInventory();
            foreach (var item in items)
            {
                var equipItem = _hotbarItems.FindNecessaryItemInData(item.ItemData.Id);
                if (equipItem != null)
                {
                    _hotbarItems.AddItem(equipItem.Data, item.Amount);
                }
            }
        }
    }
}