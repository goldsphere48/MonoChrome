using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.Components
{
    public class AudioPlayer : Component
    {
        public SoundEffect Source 
        {
            get => _soundEffect; 
            set 
            {
                _soundEffect = value;
                _instance = _soundEffect.CreateInstance();
            } 
        }

        public bool IsLooped 
        { 
            get => _isLooped;
            set
            {
                _isLooped = value;
                _instance.IsLooped = _isLooped;
            }
        }

        public bool PlayOnStart { get; set; }

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                _instance.Volume = _volume;
            }
        }

        private SoundEffect _soundEffect;
        private SoundEffectInstance _instance;
        private bool _isLooped = false;
        private float _volume;

        public void Play()
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Play();
            } else
            {
                throw new InvalidOperationException("Can't play sound, sound effect is null or disposed");
            }
        }

        public void Stop()
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Stop();
            }
            else
            {
                throw new InvalidOperationException("Can't stop play sound, sound effect is null or disposed");
            }
        }

        public void Pause()
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Pause();
            }
            else
            {
                throw new InvalidOperationException("Can't pause play sound, sound effect is null or disposed");
            }
        }

        private void Start()
        {
            if (PlayOnStart)
            {
                Play();
            }
        }

        private void OnDestroy()
        {
            _instance.Dispose();
        }
    }
}
