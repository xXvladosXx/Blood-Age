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
    [RequireComponent(typeof(ItemPicker))]

    public class ItemEquipper : MonoBehaviour, IModifier
    {
        [SerializeField] private Transform _handR;
        [SerializeField] private Transform _handL;
        [SerializeField] private StandardWeapon _starterWeapon;
        [SerializeField] private StandardWeapon _weapon;
        [SerializeField] private Projectile _standartProjectile;
        [SerializeField] private Arrow _projectile;
        public Arrow GetProjectile => _projectile;

        private GameObject _weaponPrefab;
        private Animator _animator;
        public bool IsRanged => _isRanged;

        private float _attackRange;
        public float GetAttackRange => _attackRange;
        private bool _isRanged;

        private Inventory _inventory;
        private ItemPicker _itemPicker;

        public event Action OnEquipmentChanged;

        [SerializeField] private List<Item> _inventoryItems;
        [SerializeField] private List<Item> _equippedItems = new List<Item>();
        public List<Item> GetEquippedItems => _equippedItems;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _itemPicker = GetComponent<ItemPicker>();
            
            if(_itemPicker.GetInventory != null)
                _inventoryItems = _itemPicker.GetInventory.GetInventoryItems();
        }

        private void Update()
        {
            if(_itemPicker.GetInventory == null) return;
            
            _inventoryItems = _itemPicker.GetInventory.GetInventoryItems();
        }

        private void Start()
        {
            if(_weapon == null)
                EquipItem(_starterWeapon as IEquipable);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IEquipable itemToEquip)) return;

            switch (itemToEquip)
            {
                case StandardWeapon standardWeapon:
                    EquipWeapon(standardWeapon);
                    break;
                
            }

            OnEquipmentChanged?.Invoke();
        }

        public void EquipItem(IEquipable item)
        {
            switch (item)
            {
                case Projectile projectile:
                    EquipProjectile(projectile);
                    break;
                case StandardWeapon standardWeapon:
                    EquipWeapon(standardWeapon);
                    break;
            }

            OnEquipmentChanged?.Invoke();
        }

        public void UnequipItem(IEquipable item)
        {
            switch (item)
            {
                case StandardWeapon standardWeapon:
                    UnequipWeapon(standardWeapon);
                    break;
            }
            
            OnEquipmentChanged?.Invoke();
        }

        private void UnequipWeapon(StandardWeapon standardWeapon)
        {
            var equippedItems = new List<Item>(_equippedItems);
            
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

        private void EquipProjectile(Projectile item)
        {
            if (_weapon is IRangeable weaponProjectile)
            {
                _equippedItems.Add(_projectile);
                _inventoryItems.Remove(_projectile);
            }
            else
            {
                _equippedItems.Remove(_projectile);
                _inventoryItems.Add(_projectile);
            }
        }

        private void EquipWeapon(StandardWeapon standardWeapon)
        {
            _equippedItems.Remove(_weapon);
            _inventoryItems.Add(_weapon);
            _weapon = standardWeapon;
            Destroy(_weaponPrefab);
            _equippedItems.Add(_weapon);
            _weaponPrefab = Instantiate(_weapon.GetPrefab, _weapon.IsRightHand ? _handR : _handL);

            _isRanged = _weapon.GetIsRanged;
            _attackRange = _weapon.GetAttackDistance;

            _weapon.EquipWeapon(_animator);
        }


        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IEnumerable<IBonus> AllModifierBonuses(IModifier modifier)
                => modifier.AddBonus(characteristics);

            return _equippedItems
                .OfType<IModifier>()
                .SelectMany(AllModifierBonuses);
        }
    }
}