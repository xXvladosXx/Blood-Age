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

        public void SetInfoForArrow(AttackData attackData)
        {
            _damager = attackData.Damager;
            _attackData = attackData;
            _target = attackData.PointTarget;
        }

        private void Awake()
        {
            // _cinemachineVirtualCamera =
            //     GameObject.FindWithTag("CinemachineShake").GetComponent<CinemachineVirtualCamera>();
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
                _target.GetComponent<CapsuleCollider>().height / 2 + _target.transform.position.y, _target.transform.position.z) - transform.position;

            transform.forward = direction;
        }

        private void Update()
        {
            transform.position += transform.forward * Time.deltaTime * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            print(other);
            if (other.TryGetComponent(out AliveEntity aliveEntity) && aliveEntity == _target)
            {
                if(_attackData != null)
                    aliveEntity.GetHealth.TakeHit(_attackData);
                if (_particle != null)
                {
                    var collisionEffect = Instantiate(_particle, new Vector3(aliveEntity.transform.position.x,
                            aliveEntity.GetComponent<CapsuleCollider>().height / 2 + _target.transform.position.y, aliveEntity.transform.position.z),
                        Quaternion.identity);

                    Destroy(collisionEffect, _time);
                }

                Destroy(gameObject);
            }
        }
    }
}