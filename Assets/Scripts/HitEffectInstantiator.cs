namespace DefaultNamespace
{
    using System;
    using UnityEngine;

    public class HitEffectInstantiator : MonoBehaviour
    {
        private GameObject _particleHit;
        private Transform _damager;

        private void Update()
        {
            if(_particleHit == null) return;
            if(_damager == null) return;

            _particleHit.transform.LookAt(_damager.position);
        }

        public void GenerateParticleEffectHit(GameObject particleEffect, Transform damager)
        {
            DestroyParticleHit();
            _particleHit = Instantiate(particleEffect, transform);
            _damager = damager;
        }

        public void DestroyParticleHit()
        {
            if(_particleHit == null) return;
            
            Destroy(_particleHit);
        }
    }
}