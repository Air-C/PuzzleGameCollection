using System;
using System.Collections.Generic;
using DG.Tweening;
using PGCRefactor.Enum;
using PGCRefactor.InfrastructureLayer.AssetsLoadModule.Interface;
using PGCRefactor.PresenterLayer.UIModule.Interface;
using PGCRefactor.AssetsLoadModule.Until;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace PGCRefactor.PresenterLayer.UIModule
{
    /**
     * 控制UI的生成/绑定/显示/隐藏
     */
    public class UIManager : ILoadingPageUI, IMainMenuUI
    {

        // inject module
        private IAssetsProvider _ap;
        private UICommander _uiCommander;

        //inject ui
        private Transform _canvas;
        private GameObject _mainMenuUI;
        private MainMenuListener _mainMenuListener;

        private Dictionary<PrefabEnumPopup, GameObject> _popups = new ();
        private Dictionary<PrefabEnumPopup, PopupListener> _popupsListeners = new ();
        
        private GameObject _loadingPage;
        private Slider _loadingPageSlider;
        private TMP_Text _loadingPageText;
        private bool _isInit;
        
        private static UIManager _instance;
        
        public UIManager(IAssetsProvider ap, Transform canvas, GameObject loadingPage, UICommander uiCommander, GameObject mainMenuUI = null)
        {
            _ap ??= ap;
            _uiCommander = uiCommander;
            _canvas = canvas;
            _loadingPage = loadingPage;
            if (mainMenuUI != null)
            {
                _mainMenuUI = mainMenuUI;
                InitMainMenuUI();
            }
            _loadingPageSlider = loadingPage.transform.Find("Slider").GetComponent<Slider>();
            _loadingPageText = _loadingPageSlider.transform.Find("Text").GetComponent<TMP_Text>();
            _isInit = true;
        }

        public void SetLoadingPageSlider()
        {
            _loadingPageSlider.value = _ap.CurrentProgress;
            _loadingPageText.text = $"Loading : {Math.Round(GetLoadingProcess())}%";
        }

        public void HideLoadingPage()
        {
            Object.Destroy(_loadingPage);
            _loadingPage = null;
            _loadingPageSlider = null;
            _loadingPageText = null;
        }

        public float GetLoadingProcess()
        {
            return _ap.CurrentProgress*100;
        }
        
        public void ShowMainMenu()
        {
            if(!_isInit) return;
            _mainMenuUI.SetActive(true);
        }

        public void InitMainMenuUI()
        {
            Debug.Log("Init Start");

            _mainMenuListener = _mainMenuUI.GetComponent<MainMenuListener>();
            _mainMenuListener.exitGameAction = _uiCommander.OnExitGame;
            _mainMenuListener.easingAction = _uiCommander.OnCasingScale;
            _mainMenuListener.volumeChanged = _uiCommander.OnAudioVolumeChange;
            _mainMenuListener.musicToggle = _uiCommander.OnMusicToggleChange;
            _mainMenuListener.missionChanged = _uiCommander.OnMissionChanged;
            Debug.Log("Init End");
        }

        public void ShowPopup(PrefabEnumPopup popupEnum)
        {
            if (!_popups.ContainsKey(popupEnum))
            {
                GameObject popupPrefab = _ap.GetPrefabAsset(PGCUntil.ToPrefabEnum(popupEnum));
                _popups.Add(popupEnum, Object.Instantiate(popupPrefab, _canvas));
                _popupsListeners.Add(popupEnum, _popups[popupEnum].GetComponent<PopupListener>());
                InitPopup(popupEnum);
            }
            _popups[popupEnum].SetActive(true);
        }

        public void InitPopup(PrefabEnumPopup popupEnum)
        {
            Dictionary<string, Action> popupActions = new Dictionary<string, Action>();
            Action onClick;
            foreach (Button button in _popupsListeners[popupEnum].ButtonList)
            {
                if (button.name == ButtonEnum.ExitButton.ToString())
                {
                    onClick = () =>
                    {
                        // ctx.eventBus.Publish(new ReturnMenuEvent());
                        _popups[popupEnum].gameObject.SetActive(false);
                        _popups[popupEnum].transform.DOScale(1f, 0.5f);
                    };
                }
                else if (button.name == ButtonEnum.RestartButton.ToString())
                {
                    onClick = () =>
                    {
                        // ctx.eventBus.Publish(new ReInitGameEvent());
                        _popups[popupEnum].gameObject.SetActive(false);
                        _popups[popupEnum].transform.DOScale(1f, 0.5f);
                    };
                }
                else
                {
                    onClick = () =>
                    {
                        // ctx.gameSystemState.isRunning = true;
                        _popups[popupEnum].gameObject.SetActive(false);
                        _popups[popupEnum].transform.DOScale(1f, 0.5f);
                    };
                }
                popupActions.Add(button.name, onClick);
                
            }
            
            _popupsListeners[popupEnum].Init(popupActions);
        }

        public void SetStartGameAction(Action action)
        {
            _mainMenuListener.startGameAction = action;
        }

        public void HideMainMenu()
        {
            _mainMenuUI?.SetActive(false);
        }
    }
}