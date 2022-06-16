using System;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private Sound[] _sounds;

        private void Awake()
        {
            Instance = this;
            foreach (var sound in _sounds)
            {
                sound.Source =  gameObject.AddComponent<AudioSource>();
                sound.Source.clip = sound.GetClip;

                sound.Source.outputAudioMixerGroup = sound.GetAudioMixer;
                sound.Source.volume = sound.GetVolume;
                sound.Source.pitch = sound.GetPitch;
                sound.Source.loop = sound.GetLoop;
            }
            
            Play("Theme");
        }

        public void Play(string name)
        {
            Sound s = Array.Find(_sounds, sound => sound.GetName == name);
            
            if(s != null)
                s.Source.Play();
        }
    }
}
