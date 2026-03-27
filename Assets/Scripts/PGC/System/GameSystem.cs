using System.Linq;
using Controller;
using PGC.ModelEvent.Data;
using PGC.ModuleClearLine;
using PGC.ModuleMove;
using PGC.System;
using PGC.VFX;
using ScoreSystem = PGC.ModuleScore.ScoreSystem;

namespace PGC
{

    using UnityEngine;
    
    public class GameSystem
    {

        MoveSystem moveSystem;
        ClearLineSystem clearLineSystem;
        ScoreSystem scoreSystem;
        RenderSystem renderSystem;
        VFXSystem vfxSystem;

        private float restTime;
        private float tickTime;
        MissionController missionController;
        private GameContext ctx;

        
        
        public GameSystem(GameContext ctx)
        {
            this.ctx = ctx;
            InitSystem();
            SubscribeEvents();
            tickTime = ctx.assetModule.sysSettings.gameSystemTickTime;
            Debug.Log($"System tick time: {tickTime}");
        }

        void InitSystem()
        {
            moveSystem = new MoveSystem(ctx);
            clearLineSystem = new ClearLineSystem(ctx);
            missionController = new MissionController(ctx);
            scoreSystem = new ScoreSystem(ctx);
            renderSystem = new RenderSystem(ctx);
            vfxSystem = new VFXSystem(ctx);
        }
        
        void SubscribeEvents()
        {
            ctx.eventBus.Subscribe<PreviewShapeChangeEvent>(renderSystem.OnResetShapePreview);
            ctx.eventBus.Subscribe<LineClearedEvent>(vfxSystem.OnLineCleared);
            ctx.eventBus.Subscribe<GameOverEvent>(OnGameOver);
            ctx.eventBus.Subscribe<RestartGameEvent>(OnRestartGame);
            ctx.eventBus.Subscribe<ReturnMenuEvent>(OnReturnMenu);
        }

        public void InitGame()
        {
            ctx.gameSystemState.isRunning = true;
        }
        
        public void Tick()
        {
            float dt = Time.deltaTime;
            restTime += dt;
            while (restTime >= tickTime)
            {
                moveSystem.Tick();
                restTime -= tickTime;
            }
            
        }

        public void UpdateGame()
        {
            missionController.updateMission();
            ctx.inputModule.Update(Time.deltaTime);
            clearLineSystem.ExecuteClear();
            scoreSystem.ScoreClearSquare();
            RenderSystem.RenderDestroySquare(ctx);
            RenderSystem.RenderShapePos(ctx);

        }

        public void PauseGame()
        {
            ctx.gameSystemState.isRunning = false;
            renderSystem.ShowPausePopup();
        }
        
        
        public void OnGameOver(GameOverEvent e)
        {
            ctx.gameSystemState.isRunning = false;
            renderSystem.ShowGameOverPopup();
        }

        void OnReturnMenu(ReturnMenuEvent e)
        {
            Debug.Log("todo OnReturnMenu");
        }
        
        void OnRestartGame(RestartGameEvent e)
        {
            RestartGame();
        }
        
        public void RestartGame()
        {
            //todo data init
            ResetGameData();
            ctx.gameSystemState.isRunning = true;
        }

        void ResetGameData()
        {
            if (ctx.currentSquareShape.GetSquares().Count() > 0)
            {
                foreach (var squareEntity in ctx.currentSquareShape.GetSquares())
                {
                    Object.Destroy(squareEntity.SquareObj);
                }
            }
            Debug.Log($"currentSquareShape Count: {ctx.currentSquareShape.GetSquares().Count()}");
            ctx.currentSquareShape.ClearSquares();
            ctx.NewReacdhedSquare.Clear();
            ctx.SquaresWaitForDestory.squares.Clear();
            ctx.CurrentShapeIsReachedBottom = false;
            ctx.particlePool.DeactivateParticlesForce();
            
            for (int i = 0; i < ctx.grid.GridBorder.x; i++)
            {
                for (int j = 0; j < ctx.grid.GridBorder.y + ctx.assetModule.gridSo.PreHeight; j++)
                {
                    if (ctx.grid.Get(i, j) != null)
                    {
                        Object.Destroy(ctx.grid.Get(i, j).SquareObj);
                        ctx.grid.Set(i,j,null);
                    }
                }
            }

        }
        
    }
}