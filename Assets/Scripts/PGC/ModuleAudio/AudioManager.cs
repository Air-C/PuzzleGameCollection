using PGC.Enum;
using PGC.ModelEvent.Data;
using UnityEngine;

namespace PGC.ModuleAudio
{
    public class AudioManager
    {
        private AudioSource audioSource;
        private GameContext ctx;

        public AudioManager(GameContext ctx)
        {
            this.ctx = ctx;
        }

        public void SetAudioSource(AudioSource source)
        {
            audioSource = source;
        }

        public void PlayAudio(AudiosEnum audioType)
        {
            if (audioSource == null) return;

            if (ctx.assetModule.audioClipDic.TryGetValue(audioType, out var audioClip))
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        public void OnBackGroundMusicSwitch(BackGroundMusicSwitchEvent e)
        {
            if (e.switchOn)
            {
                PlayBackgroundMusic(AudiosEnum.BackMusic);
            }
            else
            {
                PauseAudio();
            }
        }

        public void PlayBackgroundMusic(AudiosEnum audioType, bool loop = true)
        {
            if (audioSource == null) return;

            if (ctx.assetModule.audioClipDic.TryGetValue(audioType, out var audioClip))
            {
                audioSource.clip = audioClip;
                audioSource.loop = loop;
                audioSource.Play();
            }
        }

        public void PauseAudio()
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        public void ResumeAudio()
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.UnPause();
            }
        }

        public void StopAudio()
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        public void OnSetVolume(VolumeChangeEvent e)
        {
            SetVolume(e.volume);
        }
        
        public void SetVolume(float volume)
        {
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }
    }
}