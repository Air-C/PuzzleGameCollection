using System;
using PGC.AssetsLoadModule.EasingModule;
using PGCRefactor.PresenterLayer.AudioModule;
using PGCRefactor.GameLogicModule.FSM.Enum;
using PGCRefactor.GameLogicModule.FSM.Interface;
using PGCRefactor.Interface;
using UnityEngine;

namespace PGCRefactor.PresenterLayer.UIModule
{
    public class UICommander
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IAudioManager _audioManager;
        
        public UICommander(ICoroutineRunner coroutineRunner, IAudioManager audioManager)
        {
            _coroutineRunner = coroutineRunner;
            _audioManager = audioManager;
        }

        public void OnExitGame()
        {
            Application.Quit();
        }

        public void OnCasingScale(Transform settingsPanel)
        {
            _coroutineRunner.Run(DoTween.ScaleTween(settingsPanel, 0.8f, settingsPanel.localScale));
        }

        public void OnAudioVolumeChange(float volume)
        {
            _audioManager.SetAudioVolume(volume);
        }
        
        public void OnMusicToggleChange(bool isOn)
        {
            if (isOn)
            {
                _audioManager.PlayAudio();
            }
            else
            {
                _audioManager.StopAudio();
            }
        }

        public void OnMissionChanged(int value)
        {
            
        }
    }
}