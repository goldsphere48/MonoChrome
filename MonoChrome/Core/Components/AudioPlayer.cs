using Microsoft.Xna.Framework.Audio;
using System;

namespace MonoChrome.Core.Components
{
    public class AudioPlayer : Component
    {
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
        public SoundEffect Source
        {
            get => _soundEffect;
            set
            {
                _soundEffect = value;
                _instance = _soundEffect.CreateInstance();
            }
        }
        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                _instance.Volume = _volume;
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
        public void Play()
        {
            if (_instance != null && !_instance.IsDisposed)
            {
                _instance.Play();
            }
            else
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
        private SoundEffectInstance _instance;
        private bool _isLooped = false;
        private SoundEffect _soundEffect;
        private float _volume;
        private void OnDestroy()
        {
            _instance.Dispose();
        }
        private void Start()
        {
            if (PlayOnStart)
            {
                Play();
            }
        }
    }
}