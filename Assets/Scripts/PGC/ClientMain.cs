using System.Collections;
using Entities;
using PGC.Entities.Grid;
using PGC.ModelEvent;
using PGC.ModelEvent.Data;
using PGC.ModuleAsset;
using PGC.ModuleInput;
using PGC.Pool;
using PGC.System;
using PGC.VFX;
using UnityEngine;
using UnityEngine.UIElements;

namespace PGC
{
    public class ClientMain : MonoBehaviour
    {
        GameContext ctx = new GameContext();
        bool isAssetLoaded;
        GameSystem gameSystem;
        private VFXSystem vfxSystem;
        
        private void Awake()
        {
            ctx.assetModule = new AssetModule();
            ctx.inputModule = new InputModule();
            ctx.squareShape = new SquareShapeEntity();
            ctx.eventBus = new EventBus();
            StartCoroutine(PreLoadAssets());

        }

        private void Start()
        {
            
        }

        void Update()
        {
            if (!isAssetLoaded || ctx.isPaused)
            {
                return;
            }
            gameSystem.InitGame(ctx);
            gameSystem.Tick(ctx);
            gameSystem.UpdateGame(ctx);
            if (ctx.hasActiveParticles)
            {
                StartCoroutine(ctx.particlePool.DeactiveParticle(ctx));
            }
            
        }

        IEnumerator PreLoadAssets()
        {
            yield return ctx.assetModule.LoadAllAssets();
            // GameSystem.NewGame()
            ctx.grid = new GridEntity(ctx);
            ctx.particlePool = new ParticlePool(ctx);
            vfxSystem = new VFXSystem(ctx);
            gameSystem = new GameSystem(ctx);
            ctx.eventBus.Subscribe<LineClearedEvent>(vfxSystem.OnLineCleared);
            isAssetLoaded = true;
            Debug.Log($"Assets Loaded:{isAssetLoaded}");
        }
        
        public void PauseGame()
        {
            ctx.isPaused = !ctx.isPaused;
            Button pauseButton = GameObject.Find("PauseGame").GetComponent<Button>();
            
            
        }
    }
}