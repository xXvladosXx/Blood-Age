using System;
using UnityEngine;

namespace Audio
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private string _soundName;
        private AudioManager AudioManager => AudioManager.Instance;
        private void Start()
        {
            AudioManager.Play(_soundName);
        }
    }
}