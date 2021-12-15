namespace DefaultNamespace.Entity
{
    using System;
    using System.Collections.Generic;
    using DefaultNamespace.UI.Stats;
    using InventorySystem;
    using UnityEngine;

    [RequireComponent(typeof(StarterAssetsInputs))]
    [RequireComponent(typeof(ItemEquipper))]

    public class PlayerUIInventoryController : MonoBehaviour
    {
        [SerializeField] private UserInterface[] _interfaces;
        [SerializeField] private StatsPanel _statsPanel;

        private StarterAssetsInputs _starterAssetsInputs;
        private AliveEntity _aliveEntity;
        private ItemEquipper _itemEquipper;
        private DynamicInterface _dynamicInterface;

        private bool _itemDrag;
        private bool _interfaceEnter;

        private void Awake()
        {
            _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            _itemEquipper = GetComponent<ItemEquipper>();
            _aliveEntity = GetComponent<AliveEntity>();
            
            _statsPanel.SetStats(_aliveEntity.GetStats);

            foreach (var userInterface in _interfaces)
            {
                userInterface.OnInterfaceEnter += () => _interfaceEnter = true;
                userInterface.OnInterfaceExit += () => _interfaceEnter = false;
                userInterface.OnItemDrag += () => _itemDrag = true;
                userInterface.OnItemPlace += () => _itemDrag = false;
                
                if(userInterface is DynamicInterface dynamicInterface)
                    dynamicInterface.OnItemOverlap += AddInfoAboutEquipment;
            }
        }

        private void AddInfoAboutEquipment(Item item)
        {
            if(item == null) return;

            var equippedItemsWithCategory = new List<Item>();
            foreach (var equippedItem in _itemEquipper.GetEquippedItems)    
            {
                if (item.Category == equippedItem.Category)
                {
                    equippedItemsWithCategory.Add(equippedItem);
                }
            }
            
            print(equippedItemsWithCategory.Count);
            ItemTooltip.Instance.AddInfo(equippedItemsWithCategory);
        }

        private void Start()
        {
            CreateUI();
        }

        private void Update()
        {
            if (_interfaceEnter || _itemDrag)
            {
                _starterAssetsInputs.enabled = false;
            }
            else
            {
                _starterAssetsInputs.enabled = true;
            }
        }

        private void CreateUI()
        {
            var mouseData = new MouseData();

            foreach (var userInterface in _interfaces)
            {
                userInterface.SetMouseData(mouseData);
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

        
    }
}