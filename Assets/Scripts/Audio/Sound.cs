using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    [Serializable]
    public class Sound
    {
        [SerializeField] private string _name;
        [SerializeField] private AudioClip _clip;
        [Range(0,1)]
        [SerializeField] private float _volume;
        [Range(0.1f,3)]
        [SerializeField] private float _pitch;

        [SerializeField] private AudioMixerGroup _audioMixer;
        [SerializeField] private bool _loop;
        
        public AudioSource Source { get; set; }
        public float GetVolume => _volume;
        public float GetPitch => _pitch;
        public AudioClip GetClip => _clip;
        public string GetName => _name;
        public bool GetLoop => _loop;
        public AudioMixerGroup GetAudioMixer => _audioMixer;
    }
}