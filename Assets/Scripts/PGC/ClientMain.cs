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
        GameContext ctx = new ();
        bool isAssetLoaded;
        GameSystem gameSystem;
        SaveDataSystem saveSystem;
        
        private void Awake()
        {
            ctx.assetModule = new AssetModule();
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
        }

        private void Start()
        {
            
        }

        void Update()
        {
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
            gameSystem = new GameSystem(ctx);
            isAssetLoaded = true;
            gameSystem.InitGame();
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