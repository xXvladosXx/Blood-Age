using System;
using System.Collections.Generic;
using Entity;
using InventorySystem;
using StateMachine;
using StateMachine.BaseStates;
using UnityEngine;

namespace AttackSystem.AttackMechanics
{
    public class AttackMaker : MonoBehaviour
    {
        [SerializeField] private SphereCollider _sphereCollider;
        [SerializeField] private Transform _arrowSpawnPosition;
        [SerializeField] private float _disable = 0.2f;

        private float _time = -1;
        private AttackData _attackData;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }

        private void Update()
        {
            if (_time < 0)
            {
                DisableCollider();
                return;
            }

            _time -= Time.deltaTime;
        }

        private void DisableCollider()
        {
            _sphereCollider.enabled = false;
        }

        public void ActivateCollider(AttackData attackData)
        {
            _sphereCollider.enabled = true;
            _attackData = attackData;
            _time = _disable;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_attackData == null) return;
            if (other.TryGetComponent(out AliveEntity aliveEntity) && aliveEntity != _attackData.Damager)
            {
                _attackData.Entities = new List<AliveEntity> {aliveEntity};
                aliveEntity.GetHealth.TakeHit(_attackData);
            }
            
        }

        public void MakeShoot(AttackData attackData, ItemEquipper itemEquipper)
        {
            var arrow = Instantiate(itemEquipper.GetProjectile.GetPrefab, _arrowSpawnPosition.position,
                Quaternion.identity);

            arrow.GetComponent<ArrowMover>().SetInfoForArrow(attackData);
        }

        public void MakeHit(AttackData calculateAttackData)
        {
            _attackData = calculateAttackData;
            if (_attackData.PointTarget != null)
            {
                _attackData.PointTarget.GetHealth.TakeHit(_attackData);
            }
        }
    }
}