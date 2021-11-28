namespace AttackSystem
{
    using System;
    using System.Collections;
    using Cinemachine;
    using DefaultNamespace.MouseSystem;
    using UnityEngine;

    public class ArrowMover : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private AttackData _attackData;
        private Health _damager;
        private Transform _target;
        
       
        
        public void SetInfoForArrow(Health damager, AttackData attackData)
        {
            _damager = damager;
            _attackData = attackData;
            _target = attackData.Target;
        }

        private void Awake()
        {
            _cinemachineVirtualCamera =
                GameObject.FindWithTag("CinemachineShake").GetComponent<CinemachineVirtualCamera>();
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
                if (_damager.GetComponent<StarterAssetsInputs>() != null && _attackData.HeavyAttack)
                {
                    _cinemachineVirtualCamera.enabled = true;
                    StartCoroutine(DisableCamera());
                }
                
                health.TakeHit(_attackData);
                
                Destroy(gameObject);
            }
        }

        private IEnumerator DisableCamera()
        {
            yield return new WaitForSeconds(.2f);
            _cinemachineVirtualCamera.enabled = false;
        }

    }
}