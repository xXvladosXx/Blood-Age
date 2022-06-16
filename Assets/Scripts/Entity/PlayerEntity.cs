using System;
using System.Collections.Generic;
using InventorySystem;
using InventorySystem.Items;
using MouseSystem;
using PauseSystem;
using SaveSystem;
using ShopSystem;
using SkillSystem;
using StateMachine;
using UI.Stats;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Entity
{
    public class PlayerEntity : AliveEntity, ISavable
    {
        [SerializeField] private ItemContainer _hotbarItems;
        [SerializeField] private List<GameObject> _raycasters = new List<GameObject>();
        [SerializeField] private CursorIterating[] _cursorIteratings;
        
        [Serializable]
        struct CursorIterating
        {
            public CursorType Cursor;
            public Vector2 Hotspot;
            public Texture2D Texture2D;
        }

        private Camera _mainCamera;
        private ItemPicker _itemPicker;
        private Customer _customer;
        private PlayerInputs _playerInputs;
        private SkillTree _skillTree;

        private bool _canSave;

        public List<GameObject> GetRaycasters => _raycasters;
        public ItemPicker GetItemPicker => _itemPicker;
        public Customer GetCustomer => _customer;
        protected override void Init()
        {
            _mainCamera = Camera.main;
            _itemPicker = GetComponent<ItemPicker>();
            _customer = GetComponent<Customer>();
            _playerInputs = GetComponent<PlayerInputs>();
            _skillTree = GetComponent<SkillTree>();

            _customer.OnItemBuy += CheckAddingToHotBar;
            _itemPicker.OnItemPickUp += CheckAddingToHotBar;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            FindStats.OnLevelUp += _skillTree.AddPoints;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            FindStats.OnLevelUp -= _skillTree.AddPoints;
        }
        
        private void FixedUpdate()
        {
            if(!_playerInputs.enabled) return;
            if (ProjectContext.Instance.PauseManager.IsPaused)
            {
                Movement.Cancel();
            }
            
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                UseItem(0, this);
            }
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                UseItem(1, this);
            }
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                UseItem(2, this);
            }
            
            if(PointerOverUI()) return;
            if(InteractWithComponent()) return;
            if(OrdinaryCursor()) return;
        }
       
        private void CheckAddingToHotBar(InventoryItem obj, int amount = 1)
        {
            if (_hotbarItems.HasItemInInventory(obj.Data))
            {
                _hotbarItems.AddItem(obj.Data, _itemPicker.GetItemContainer.FindSlotInInventory(obj.Data).Amount);
                _itemPicker.GetItemContainer.RemoveItem(obj.Data.Id, _itemPicker.GetItemContainer.FindSlotInInventory(obj.Data).Amount);
            }
        }

        private void UseItem(int index, AliveEntity aliveEntity)
        {
            if (_hotbarItems.GetItemOnIndexSlot(index) is IConsumable consumable)
            {
                _hotbarItems.RemoveItem(_hotbarItems.GetItemOnIndexSlot(index).Data.Id, 1);
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

        public bool PointerOverUI()
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
                var equipItem = _hotbarItems.Database.GetItemByID(item.ItemData.Id);
                if (equipItem != null)
                {
                    _hotbarItems.AddItem(equipItem.Data, item.Amount);
                }
            }
        }
    }
}