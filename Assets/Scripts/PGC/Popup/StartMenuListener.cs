using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PGC.Popup
{
    public class StartMenuListener : MonoBehaviour
    {
        [SerializeField]
        private Button startGameButton;
        [SerializeField]
        private Button exitGameButton;
        [SerializeField]
        private Button settingsButton;
        
        [SerializeField]
        private Slider volumeSlider;
        [SerializeField]
        private TMP_Dropdown missionDropdown;
        [SerializeField]
        private TMP_InputField userNameInput;
        [FormerlySerializedAs("musicToggle")] [SerializeField]
        private Toggle backGroundMusicToggle;
        public bool isUnsetEvent = true;

        public Button StartGameButton
        {
            get => startGameButton;
            set => startGameButton = value;
        }

        public Button ExitGameButton
        {
            get => exitGameButton;
            set => exitGameButton = value;
        }

        public Button SettingsButton
        {
            get => settingsButton;
            set => settingsButton = value;
        }

        public Slider VolumeSlider
        {
            get => volumeSlider;
            set => volumeSlider = value;
        }

        public TMP_Dropdown MissionDropdown
        {
            get => missionDropdown;
            set => missionDropdown = value;
        }

        public TMP_InputField UserNameInput
        {
            get => userNameInput;
            set => userNameInput = value;
        }

        public Toggle BackGroundMusicToggle
        {
            get => backGroundMusicToggle;
            set => backGroundMusicToggle = value;
        }
    }
}