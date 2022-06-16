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
        [SerializeField] private Transform _handR;
        [SerializeField] private Transform _handL;
        [SerializeField] private StandardWeapon _starterWeapon;
        [SerializeField] private StandardWeapon _weapon;
        [SerializeField] private Projectile _standartProjectile;
        [SerializeField] private Arrow _projectile;

        private GameObject _weaponPrefab;
        private Animator _animator;
        private float _attackRange;
        private Dictionary<DamageType, float> _resistances = new Dictionary<DamageType, float>();
        
        public DamageResistance GetDamageResistance => _damageResistance;
        public Arrow GetProjectile => _projectile;
        public StandardWeapon GetCurrentWeapon => _weapon;
        public float GetAttackRange => _attackRange;
        public AttackMaker GetAttackMaker => _weaponPrefab.GetComponent<AttackMaker>();
        public event Action<InventoryItem> OnEquipmentChange;

        private void Awake()
        {
            if (_damageResistance == null)
            {
                _damageResistance = Resources.Load<DamageResistance>( _defaultResistance);
            }
        }

        private void FindStarterResistance()
        {
            foreach (var resistance in _damageResistance._settings)
            {
                if (!_resistances.TryGetValue(resistance.DamageType, out var f))
                {
                    _resistances.Add(resistance.DamageType, resistance.DamageResistance);
                }
                else
                {
                    _resistances.Remove(resistance.DamageType);
                    _resistances.Add(resistance.DamageType, f+resistance.DamageResistance);
                }
            }
        }

        private void FindItemsResistances()
        {
            foreach (var inventoryItem in _equipmentItems.GetInventoryItems())
            {
                if (inventoryItem is StandardArmor standardArmor)
                {
                    foreach (var resistance in standardArmor.GetDamageResistances)
                    {
                        if (_resistances.TryGetValue(resistance.DamageType, out var f))
                        {
                            _resistances.Remove(resistance.DamageType);
                            _resistances.Add(resistance.DamageType, f + resistance.DamageResistance);
                        }
                    }
                }
            }
        }

        public float CalculateResistance(float damage, DamageType damageType)
        {
            FindStarterResistance();
            FindItemsResistances();
            
            foreach (var resistance in _resistances)
            {
                if (resistance.Key == damageType)
                {
                    float f = damage * (1 - resistance.Value);
                    _resistances.Clear();
                    return f;
                }
            }
            
            return 0;
        }
        
        private void Update()
        {
            foreach (var damageResistanceResistance in _resistances)
            {
                print(damageResistanceResistance.Key +" " +damageResistanceResistance.Value);
            }
        }

        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
            OnInventoryUpdate();
            
            _equipmentItems.OnInventoryChange += OnInventoryUpdate;
        }

        private void OnInventoryUpdate()
        {
            var itemsInInventory = _equipmentItems.GetInventoryItems();
            
            foreach (var inventoryItem in itemsInInventory)
            {
                switch (inventoryItem)
                {
                    case StandardWeapon standardWeapon:
                        WeaponEquip(standardWeapon);
                        break;
                    case StandardArmor standardArmor:
                        OnEquipmentChange?.Invoke(standardArmor);
                        break;
                }
            }
            
            var weapon = itemsInInventory.OfType<StandardWeapon>().FirstOrDefault();
            if(weapon == null)
                WeaponEquip(_starterWeapon);
        }

        private void WeaponEquip(StandardWeapon weapon)
        {
            _weapon = weapon;
            weapon.EquipWeapon(_animator);
            Destroy(_weaponPrefab);
            _weaponPrefab = Instantiate(weapon.GetPrefab, weapon.IsRightHand ? _handR : _handL);
            _attackRange = weapon.GetAttackDistance;
            OnEquipmentChange?.Invoke(weapon);
        }

        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IEnumerable<IBonus> AllModifierBonuses(IModifier modifier)
                => modifier.AddBonus(characteristics);

            return _equipmentItems.GetInventoryItems()
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
                var equipItem = _equipmentItems.Database.GetItemByID(item.ItemData.Id);
                if (equipItem != null)
                {
                    _equipmentItems.AddItem(equipItem.Data, item.Amount);
                }
            }
        }
    }
}