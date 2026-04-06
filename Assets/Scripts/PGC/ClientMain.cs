using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using PGC.Entities.Grid;
using PGC.Enum;
using PGC.ModelEvent;
using PGC.ModelEvent.Data;
using PGC.ModuleAsset;
using PGC.ModuleInput;
using PGC.ModuleInventory;
using PGC.ModuleInventory.SO;
using PGC.ModuleItem.Model;
using PGC.Pool;
using PGC.System;
using PGC.VFX;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ScoreSystem = PGC.ModuleScore.ScoreSystem;

namespace PGC
{
    public class ClientMain : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> itemsBar;
        [SerializeField]
        private GameObject shapePreviewBar;
        [SerializeField]
        private GameObject pauseGameButton;
        [SerializeField]
        private GameObject startMenu;
        [SerializeField]
        private GameObject settingsBar;
        [SerializeField]
        private Image backGroundUI;
        [SerializeField]
        private Slider loadingUI;

        GameContext ctx = new ();
        bool isAssetLoaded;
        GameSystem gameSystem;
        SaveDataSystem saveSystem;
        
        private void Awake()
        {
            ctx.assetModule = new AssetModule(ctx);
            StartCoroutine(PreLoadAssets());

            ctx.inputModule = new InputModule();
            ctx.currentSquareShape = new SquareShapeEntity();
            ctx.eventBus = new EventBus();
            ctx.gameSystemState = new GameSystemState();
            saveSystem = new SaveDataSystem();
            saveSystem.LoadData();
            ctx.saveData = saveSystem.SaveData;
            ctx.inventoryLocalData = new InventoryLocalData();
            ObjectRequireCheck();
            foreach (var itemBar in itemsBar)
            {
                ctx.itemsBar.Add(new ItemBarModel()
                {
                    type = ItemAbilityType.None,
                    itemBar = itemBar
                });
            }
            ctx.shapePreviewBar = shapePreviewBar;
            ctx.pauseGameButton = pauseGameButton;
            ctx.startMenu = startMenu;
            ctx.settingsBar = settingsBar;
            ctx.backGroundUI = backGroundUI;
            ctx.loadingUI = loadingUI;
        }

        void Start()
        {
            
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

        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            if (ctx.gameSystemState.isRunning == false)
            {
                return;
            }
            gameSystem.Tick();
            gameSystem.UpdateGame();
            if (ctx.hasActiveParticles)
            {
                StartCoroutine(ctx.particlePool.DeactivateParticle());
            }
            
        }

        IEnumerator PreLoadAssets()
        {
            yield return ctx.assetModule.LoadAllAssets();
            ctx.grid = new GridEntity(ctx);
            ctx.particlePool = new ParticlePool(ctx);
            ctx.squarePool = new SquarePool(ctx);
            gameSystem = new GameSystem(ctx);
            isAssetLoaded = true;
            ctx.loadingUI.value = 1;
            gameSystem.InitGame();
            ctx.loadingUI.gameObject.SetActive(false);
            ctx.backGroundUI.transform.SetAsFirstSibling();
            Debug.Log($"Assets Loaded:{isAssetLoaded}");
        }

        void ObjectRequireCheck()
        {
            GameObjectListCheck(itemsBar, new List<Type>(){typeof(Image)}, "itemsBar");
            GameObjectCheck(shapePreviewBar, new List<Type>(){typeof(Image)}, "shapePreviewBar");
            GameObjectCheck(pauseGameButton, new List<Type>(){typeof(Button)}, "pauseGameButton");
        }

        void GameObjectListCheck(List<GameObject> objs, List<Type> components, string warnMsg = "GameObjectSetCheck")
        {
            foreach (var obj in objs)
            {
                GameObjectCheck(obj, components, warnMsg);
            }
        }
        
        void GameObjectCheck(GameObject obj, List<Type> components, string warnMsg = "GameObjectSetCheck")
        {
            if (obj == null )
            {
                Debug.LogWarning($"{warnMsg}: GameObject is null");
                return;
            }

            foreach (var component in components)
            {
                if (obj.GetComponent(component) == null)
                {
                    Debug.LogWarning($"{warnMsg}: Component {component} not exists");
                }
            }
            
        }

    }
}