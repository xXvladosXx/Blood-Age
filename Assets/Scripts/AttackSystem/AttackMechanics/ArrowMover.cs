namespace AttackSystem
{
    using System;
    using DefaultNamespace.MouseSystem;
    using UnityEngine;

    public class ArrowMover : MonoBehaviour
    {
        private float _speed;
        private Health _damager;
        private Transform _target;
        private float _damage;
        public void SetInfoForArrow(float speed, Health damager, Transform attackDataTarget, float damage)
        {
            _speed = speed;
            _damager = damager;
            _target = attackDataTarget;
            _damage = damage;
        }

        private void Start()
        {
            Vector3 direction = new Vector3(_target.transform.position.x, 
                _target.GetComponent<CapsuleCollider>().height/2, _target.transform.position.z) - transform.position;
            
            transform.forward = direction;
        }

        private void Update()
        {
           transform.position += transform.forward * Time.deltaTime * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Health health) && health != _damager)
            {
                health.TakeHit(new AttackData
                {
                    Damage = _damage,
                    Target = _target
                });
                
                Destroy(gameObject);
            }
        }
    }
}