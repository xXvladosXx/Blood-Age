using System.Collections;
using Cinemachine;
using Entity;
using UnityEngine;

namespace AttackSystem.AttackMechanics
{
    public class ArrowMover : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private GameObject _particle;
        [SerializeField] private float _time;
        [SerializeField] private AliveEntity _target;

        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private AttackData _attackData;
        private AliveEntity _damager;
        private bool _stopped;

        public void SetInfoForArrow(AttackData attackData)
        {
            _damager = attackData.Damager;
            _attackData = attackData;
            _target = attackData.PointTarget;
        }

        private void Start()
        {
            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }

            Destroy(gameObject, 4f);
            Vector3 direction = new Vector3(_target.transform.position.x,
                _target.GetComponent<CapsuleCollider>().height / 2 + _target.transform.position.y,
                _target.transform.position.z) - transform.position;

            transform.forward = direction;
        }

        private void Update()
        {
            if(_stopped) return;
            
            transform.position += transform.forward * Time.deltaTime * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable destroyable))
            {
                if (_attackData != null)
                {
                    if ((AliveEntity) destroyable == _target)
                    {
                        destroyable.Damage(_attackData);
                    }
                    else
                    {
                        return;
                    }
                }

                if (_particle != null)
                {
                    var collisionEffect = Instantiate(_particle, new Vector3(_target.transform.position.x,
                            _target.GetComponent<CapsuleCollider>().height / 2 + _target.transform.position.y,
                            _target.transform.position.z),
                        Quaternion.identity);

                    Destroy(collisionEffect, _time);
                }
                
                _stopped = true;
               
                Destroy(gameObject, 3f);
            }
        }
    }
}