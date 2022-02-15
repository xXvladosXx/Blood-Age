using System;
using Cinemachine;
using Entity;
using InventorySystem;
using StatsSystem;
using UnityEngine;

namespace AttackSystem.AttackMechanics
{
    public class AttackRegister
    {
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private Transform _arrowSpawnPosition;

        private float _arrowSpeed = 15f;
        private float _stickSpeed = 15f;
        private AttackData _attackData;
        public AttackData GetAttackData => _attackData;
        private SphereCollider _sphereCollider;

        public AttackRegister()
        {
            _attackData = new AttackData();
        }

        public AttackData CalculateAttackData(FindStats findStats, AliveEntity aliveEntity, ItemEquipper itemEquipper)
        {
            _attackData.Damager = aliveEntity;
            _attackData.Damage = findStats.GetStat(Characteristics.Damage);
            _attackData.CriticalChance = findStats.GetStat(Characteristics.CriticalChance);
            _attackData.CriticalDamage = findStats.GetStat(Characteristics.CriticalDamage);
            _attackData.ElementalDamage = itemEquipper.GetCurrentWeapon.GetDamageDictionary;
            _attackData.Accuracy = findStats.GetStat(Characteristics.Accuracy);
            _attackData.Targets = aliveEntity.Targets;
                
            return _attackData;
        }
    }
}