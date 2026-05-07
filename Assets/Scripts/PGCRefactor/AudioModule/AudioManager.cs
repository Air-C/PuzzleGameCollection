using PGCRefactor.Enum;
using PGCRefactor.ModuleAsset.Interface;
using UnityEngine;

namespace PGCRefactor.AudioModule
{
    public class AudioManager : IAudioManager
    {
        private IAssetsProvider _ap;
        private AudioSource _audioSource;

        public AudioManager(IAssetsProvider ap)
        {
            _ap = ap;
        }
        
        public void Initialize()
        {
            if (_audioSource == null)
            {
                GameObject prefab = _ap.GetPrefabAsset(PrefabEnum.AudioSourcePrefab);
                _audioSource = Object.Instantiate(prefab)?.GetComponent<AudioSource>();
            }
        }

        public void PlayAudio()
        {
            _audioSource.Play();
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
        }

        public void StopAudio()
        {
            _audioSource.Stop();
        }

        public void SetAudioVolume(float volume)
        {
            _audioSource.volume = volume;
        }
    }
}