using System;
using StatsSystem;
using UnityEngine;

namespace CharacterSelecting
{
    public class CharacterHoverLight : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        
        private CharacterControl _hoverSelectedCharacter;
        private Vector3 _targetPos;
        private Light _light;

        private void Awake()
        {
            _light = GetComponent<Light>();
        }

        private void Update()
        {
            if (_hoverSelectedCharacter != null)
            {
                LightUpSelectedCharacter();
            }
        }

        public void ResetLight()
        {
            if(!_light.enabled) return;
            
            _hoverSelectedCharacter = null;
            _light.enabled = false;
        }

        public void SetLight(CharacterControl characterControl)
        {
            if(_light.enabled) return;

            _hoverSelectedCharacter = characterControl;
            _light.enabled = true;
        }

        private void LightUpSelectedCharacter()
        {
              transform.position = _hoverSelectedCharacter.transform.position +
                                     _hoverSelectedCharacter.transform.TransformDirection(_offset);
        }
    }
}