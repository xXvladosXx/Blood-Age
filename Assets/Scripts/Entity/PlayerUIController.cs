namespace DefaultNamespace.Entity
{
    using System;
    using InventorySystem;
    using UnityEngine;

    [RequireComponent(typeof(StarterAssetsInputs))]
    [RequireComponent(typeof(ItemEquipper))]

    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField] private UserInterface[] _interfaces;

        private StarterAssetsInputs _starterAssetsInputs;
        private ItemEquipper _itemEquipper;

        private void Awake()
        {
            _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            _itemEquipper = GetComponent<ItemEquipper>();
            
            CreateUI();
        }
        
        private void CreateUI()
        {
            var mouseData = new MouseData();

            foreach (var userInterface in _interfaces)
            {
                userInterface.SetMouseData(mouseData);
                // userInterface.OnItemDrag += DisableMovement;
                // userInterface.OnItemPlace += EnableMovement;
                if (userInterface is StaticInterface staticInterface)
                {
                    foreach (var item in staticInterface.GetInventory.GetInventoryItems())
                    {
                        EquipItem(item);
                    }
                }
                
                userInterface.OnItemEquip += EquipItem;
                userInterface.OnItemUnequip += UnequipItem;
            }
        }

        private void OnDisable()
        {
            foreach (var userInterface in _interfaces)
            {
                // userInterface.OnItemDrag += DisableMovement;
                // userInterface.OnItemPlace += EnableMovement;
                userInterface.OnItemEquip -= EquipItem;
                userInterface.OnItemUnequip -= UnequipItem;
            }
        }
        
        private void UnequipItem(Item obj)
        {
            if (obj is IEquipable equipable)
            {
                _itemEquipper.UnequipItem(equipable);
            }
        }

        private void EquipItem(Item obj)
        {
            if (obj is IEquipable equipable)
            {
                _itemEquipper.EquipItem(equipable);
            }
        }

        private void EnableMovement()
        {
            _starterAssetsInputs.enabled = true;
        }

        private void DisableMovement()
        {
            _starterAssetsInputs.enabled = false;
        }
    }
    
    public class MouseData
    {
        public UserInterface UI { get; set; }
        public GameObject TempItemDrag { get; set; }
        public GameObject TempItemHover { get; set; }
    }
}