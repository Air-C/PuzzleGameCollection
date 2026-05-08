using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PGCRefactor.PresenterLayer.UIModule
{
    public class MainMenuListener : MonoBehaviour
    {
        [SerializeField]
        private Image background;
        
        [SerializeField]
        private Button startGameButton;
        [SerializeField]
        private Button exitGameButton;
        [SerializeField]
        private Button settingsButton;
        [SerializeField]
        private GameObject settingsBar;
        
        [SerializeField]
        private Slider volumeSlider;
        [SerializeField]
        private TMP_Dropdown missionDropdown;
        [SerializeField]
        private TMP_InputField userNameInput;
        [SerializeField]
        private Toggle backGroundMusicToggle;
        // public bool isUnsetEvent = true;

        public Action startGameAction;
        public Action exitGameAction;
        public Action<Transform> easingAction;
        public Action<float> volumeChanged;
        public Action<int> missionChanged;
        public Action<bool> musicToggle;

        private void Awake()
        {
            backGroundMusicToggle.onValueChanged.AddListener((value) => { musicToggle?.Invoke(value); });
            
            missionDropdown.onValueChanged.AddListener((value) => { missionChanged?.Invoke(value); });
            
            volumeSlider.onValueChanged.AddListener((value) => { volumeChanged?.Invoke(value); });
            
            settingsButton.onClick.AddListener(() =>
            {
                if (settingsBar.activeSelf == false)
                {
                    settingsBar.SetActive(true);
                    easingAction?.Invoke(settingsButton.transform);
                    easingAction?.Invoke(settingsBar.transform);
                }
                else
                {
                    settingsBar.SetActive(false);
                }
            });
                        
            exitGameButton.onClick.AddListener(() =>
            {
                exitGameAction?.Invoke();
                easingAction?.Invoke(exitGameButton.transform);
            });
            
            startGameButton.onClick.AddListener(() =>
            {
                startGameAction?.Invoke();
                easingAction?.Invoke(startGameButton.transform);
            });
        }
        
        
    }
}