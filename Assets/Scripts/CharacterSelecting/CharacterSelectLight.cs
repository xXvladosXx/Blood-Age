using System;
using UnityEngine;

namespace CharacterSelecting
{
    public class CharacterSelectLight : MonoBehaviour
    {
        private Light _light;
        public Light GetLight => _light;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _light.enabled = false;
        }
    }
}