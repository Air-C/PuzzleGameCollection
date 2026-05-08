using UnityEngine;

namespace PGCRefactor.PresenterLayer.AudioModule
{
    public interface IAudioManager
    {
        public void Initialize();
        public void PlayAudio();
        public void SetAudioClip(AudioClip audioClip);
        public void StopAudio();
        public void SetAudioVolume(float volume);
    }
}