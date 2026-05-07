using System.Collections;
using System.Collections.Generic;
using PGCRefactor.AudioModule;
using PGCRefactor.System;
using PGCRefactor.GameLogicModule.FSM;
using PGCRefactor.GameLogicModule.FSM.Enum;
using PGCRefactor.GameLogicModule.GameLogic.TM;
using PGCRefactor.Interface;
using PGCRefactor.ModuleAsset;
using PGCRefactor.UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace PGCRefactor
{
    public class ClientMain : MonoBehaviour, ICoroutineRunner
    {
        // game view
        [SerializeField]
        private List<GameObject> itemsBar;
        [SerializeField]
        private GameObject shapePreviewBar;
        [SerializeField]
        private GameObject pauseGameButton;
        [SerializeField]
        private GameObject settingsBar;

        // Global view
        [SerializeField]
        private GameObject defaultCanvas;
        [SerializeField]
        private GameObject mainMenuUI;
        [SerializeField]
        private GameObject loadingPage;

        // ScriptableObject configs
        [SerializeField] private BoardSO boardSO;
        [SerializeField] private ShapeSO shapeSO;

        private readonly GameContext _ctx = new ();
        private GameSystem _gameSystem;

        
        private void Awake()
        {
            _ctx.assetsLoadModule = new AssetsLoadModule();
            StartCoroutine(PreLoadAssets());
        }

        void Start()
        {
            _ctx.audioManager = new AudioManager(_ctx.assetsLoadModule);
            UICommander uiCommander = new UICommander(this, _ctx.audioManager);
            _ctx.uiManager = new UIManager(_ctx.assetsLoadModule, defaultCanvas.transform, loadingPage, uiCommander, mainMenuUI);
            _ctx.gameFsm = new GameFsm(_ctx.uiManager,_ctx.uiManager);
            _gameSystem = new GameSystem(_ctx.gameFsm, boardSO, shapeSO);
            _gameSystem.InitializeGame();
        }
        
        
        void Update()
        {
            _gameSystem.GameLoop();
        }
        
        float deltaTime = 0.0f;
        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;
            Rect rect = new Rect(10, 10, w, h*0.05f );
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 50;
            style.normal.textColor = Color.white;
            
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0}", fps);
            GUI.Label(rect, text, style);
        }



        IEnumerator PreLoadAssets()
        {
            yield return _ctx.assetsLoadModule.LoadAllAssets();
            _ctx.assetsLoadModule.EndProgress();
        }


        private void OnApplicationQuit()
        {
            TearDown();
        }

        private void OnDestroy()
        {
            TearDown();
        }

        void TearDown()
        {
            
        }

        public Coroutine Run(IEnumerator routine) => StartCoroutine(routine);
    }
}