using AttackSystem.AttackMechanics;
using InventorySystem.Items;
using InventorySystem.Items.Armor;
using InventorySystem.Items.Projectile;
using InventorySystem.Items.Weapon;
using Resistance;
using SaveSystem;
using StatsSystem;

namespace InventorySystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AttackSystem.Weapon;
    using DefaultNamespace;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [RequireComponent(typeof(Animator))]
    public class ItemEquipper : MonoBehaviour, IModifier, ISavable
    {
        [SerializeField] private ItemContainer _equipmentItems;
        [SerializeField] private DamageResistance _damageResistance;
        [SerializeField] private string _defaultResistance = "Default Resistance";
        [SerializeField] private List<InventoryItem> _equippedItems = new List<InventoryItem>();
        [SerializeField] private Transform _handR;
        [SerializeField] private Transform _handL;
        [SerializeField] private StandardWeapon _starterWeapon;
        [SerializeField] private StandardWeapon _weapon;
        [SerializeField] private Projectile _standartProjectile;
        [SerializeField] private Arrow _projectile;

        private Chest _chest;
        private Helmet _helmet;
        private Boots _boots;
        private GameObject _weaponPrefab;
        private Animator _animator;
        private float _attackRange;
        private ItemContainer _itemContainer;
        private ItemPicker _itemPicker;
        
        public DamageResistance GetDamageResistance => _damageResistance;
        public List<InventoryItem> GetEquippedItems => _equippedItems;
        public Arrow GetProjectile => _projectile;
        public StandardWeapon GetCurrentWeapon => _weapon;
        public float GetAttackRange => _attackRange;


        public event Action<AttackMaker> OnWeaponChanged;
        public event Action OnEquipmentChange;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _itemPicker = GetComponent<ItemPicker>();
            if (_damageResistance == null)
            {
                _damageResistance = Resources.Load<DamageResistance>( _defaultResistance);
            }
            _damageResistance.SetUp();
            
            if(_itemPicker == null) return;
            
            _equipmentItems.OnItemChange += CheckToUnEquip;
            _itemPicker.GetItemContainer.OnItemChange += CheckToEquip;
        }
        
        private void Start()
        {
            if(_equipmentItems == null) return;
            foreach (var item in _equipmentItems.GetInventoryItems())   
            {
                EquipItem(item as InventoryItem);
            }
        }


        public void SetResistance(DamageResistance.Resistance[] resistances, bool b = false)
        {
            foreach (var resistance in resistances)
            {
                if (_damageResistance._resistances.TryGetValue(resistance.DamageType, out var f))
                {
                    _damageResistance._resistances.Remove(resistance.DamageType);
                    _damageResistance._resistances.Add(resistance.DamageType, f + (b ? -resistance.DamageResistance : resistance.DamageResistance));
                }
            }
        }
        private void CheckToEquip(Item item1, Item item2)
        {
            if (!_itemPicker.GetItemContainer.GetInventoryItems().Contains(item2))
            {
                EquipItem(item2 as InventoryItem);
            }
        }

        private void CheckToUnEquip(Item item1, Item item2)
        {
            if (item1 != null)
            {
                UnequipItem(item2 as IEquipable);
                EquipItem(item1 as InventoryItem);
            }
            
            if (item1 == null)
            {
                UnequipItem(item2 as IEquipable);
            }
        }

        private void EquipItem(InventoryItem item)
        {
            switch (item)
            {
                case Projectile projectile:
                    EquipProjectile(projectile);
                    break;
                case StandardWeapon standardWeapon:
                    EquipWeapon(standardWeapon);
                    break;
                case StandardArmor standardArmor:
                    EquipArmor(standardArmor);
                    break;
            }
        }

        private void UnequipItem(IEquipable item)
        {
            switch (item)
            {
                case StandardWeapon standardWeapon:
                    UnequipWeapon(standardWeapon);
                    break;
                case StandardArmor standardArmor:
                    UnequipArmor(standardArmor);
                    break;
            }
        }

        private void UnequipWeapon(StandardWeapon standardWeapon)
        {
            var equippedItems = new List<InventoryItem>(_equippedItems);

            foreach (var item in equippedItems)
            {
                if (item.Data.Id == standardWeapon.Data.Id)
                {
                    _equippedItems.Remove(standardWeapon);
                    Destroy(_weaponPrefab);
                    _weapon = _starterWeapon;
                    _weaponPrefab = Instantiate(_weapon.GetPrefab, _weapon.IsRightHand ? _handR : _handL);
                    _weapon.EquipWeapon(_animator);
                }
            }
        }
        
        private void UnequipArmor(StandardArmor standardArmor)
        {
            switch (standardArmor)
            {
                case Boots boots:
                    _equippedItems.Remove(_boots);
                    SetResistance(_boots.GetDamageResistances, true);
                    _boots = null;

                    break;
                case Chest chest:
                    _equippedItems.Remove(_chest);
                    SetResistance(_chest.GetDamageResistances, true);
                    _chest = null;

                    break;
                case Helmet helmet:
                    _equippedItems.Remove(_helmet);
                    SetResistance(_helmet.GetDamageResistances, true);
                    _helmet = null;

                    break;
            }
            
            OnEquipmentChange?.Invoke();
        }

        private void EquipProjectile(Projectile item)
        {
            if (_weapon is IRangeable weaponProjectile)
            {
                _equippedItems.Add(_projectile);
            }
            else
            {
                _equippedItems.Remove(_projectile);
            }
        }

        private void EquipWeapon(StandardWeapon standardWeapon)
        {
            _equippedItems.Remove(_weapon);
            _weapon = standardWeapon;
            Destroy(_weaponPrefab);
            _equippedItems.Add(_weapon);
            _weaponPrefab = Instantiate(_weapon.GetPrefab, _weapon.IsRightHand ? _handR : _handL);

            _attackRange = _weapon.GetAttackDistance;
            _weapon.EquipWeapon(_animator);
            OnEquipmentChange?.Invoke();
            OnWeaponChanged?.Invoke(_weaponPrefab.GetComponent<AttackMaker>());
        }
        
        private void EquipArmor(StandardArmor standardArmor)
        {
            switch (standardArmor)
            {
                case Boots boots:
                    _equippedItems.Remove(_boots);
                    _boots = boots;
                    break;
                case Chest chest:
                    _equippedItems.Remove(_chest);
                    _chest = chest;
                    break;
                case Helmet helmet:
                    _equippedItems.Remove(_helmet);
                    _helmet = helmet;
                    break;
            }

            SetResistance(standardArmor.GetDamageResistances);
            _equippedItems.Add(standardArmor);
            OnEquipmentChange?.Invoke();
        }

        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IEnumerable<IBonus> AllModifierBonuses(IModifier modifier)
                => modifier.AddBonus(characteristics);

            return _equippedItems
                .OfType<IModifier>()
                .SelectMany(AllModifierBonuses);
        }

        public object CaptureState()
        {
            var items = _equipmentItems.GetAllSlots();
            return items;
        }

        public void RestoreState(object state)
        {
            var items = state as List<Slot>;
            _equipmentItems.ClearInventory();
            foreach (var item in items)
            {
                var equipItem = _equipmentItems.FindNecessaryItemInData(item.ItemData.Id);
                if (equipItem != null)
                {
                    _equipmentItems.AddItem(equipItem.Data, item.Amount);
                    EquipItem(equipItem as InventoryItem);
                }
            }
        }
    }
}