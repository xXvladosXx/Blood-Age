namespace InventorySystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AttackSystem.Weapon;
    using DefaultNamespace;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class ItemEquipper : MonoBehaviour, IModifier 
    {
        [SerializeField] private Transform _handR;
        [SerializeField] private Transform _handL;
        [SerializeField] private StandardWeapon _weapon;
        [SerializeField] private Projectile _standartProjectile;
        [SerializeField] private Arrow _projectile;
        public Arrow GetProjectile => _projectile;
        
        private GameObject _weaponPrefab;
        private Animator _animator;
        public bool IsRanged => _isRanged;
        
        private float _attackRange;
        public float GetAttackRange => _attackRange;
        private float _attackDamage;
        private bool _isRanged;

        [SerializeField] private List<Item> _items = new List<Item>();
        [SerializeField] private List<Item> _equippedItems = new List<Item>();

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if(_weapon == null) return;
            
            _weaponPrefab = Instantiate(_weapon.GetPrefab, _weapon.IsRightHand ? _handR : _handL);

            _isRanged = _weapon.GetIsRanged;
            _attackDamage = _weapon.GetAttackDamage;
            _attackRange = _weapon.GetAttackDistance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IEquipable itemToEquip)) return;

            var item = itemToEquip.Equip();
            
            switch (item)
            {
                case StandardWeapon _:
                    EquipWeapon(itemToEquip);

                    EquipProjectile(item);
                    break;
                case StandardArmor _:
                    print("Armor");
                    break;
            }
        }

        private void EquipProjectile(Item item)
        {
            if (_weapon is IRangeable weaponProjectile && _weapon == item)
            {
                _equippedItems.Add(_projectile);
                _items.Remove(_projectile);
                print(weaponProjectile.GetProjectileType());
            }
            else
            {
                _equippedItems.Remove(_projectile);
                _items.Add(_projectile);
            }
        }

        private void EquipWeapon(IEquipable itemToEquip)
        {
            if (_weapon != null)
            {
                _equippedItems.Remove(_weapon);
                _items.Add(_weapon);
                _weapon = itemToEquip.Equip() as StandardWeapon;
                Destroy(_weaponPrefab);
                _equippedItems.Add(_weapon);
                _weaponPrefab = Instantiate(_weapon.GetPrefab, _weapon.IsRightHand ? _handR : _handL);
            }
            else
            {
                _weapon = itemToEquip.Equip() as StandardWeapon;
                _weaponPrefab = Instantiate(_weapon.GetPrefab, _weapon.IsRightHand ? _handR : _handL);
                _equippedItems.Add(_weapon);
            }
            
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