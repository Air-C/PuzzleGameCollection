using System.Linq;
using Controller;
using PGC.ModelEvent.Data;
using PGC.ModuleClearLine;
using PGC.ModuleItem;
using PGC.ModuleMove;
using PGC.System;
using PGC.ModuleAudio;
using PGC.VFX;
using UnityEngine.UI;
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
        ItemSystem itemSystem;
        AudioManager audioManager;

        private float restTime;
        private float tickTime;
        MissionController missionController;
        private GameContext ctx;

        
        
        public GameSystem(GameContext ctx)
        {
            this.ctx = ctx;
            InitSystem();
            SubscribeEvents();
            ctx.pauseGameButton.GetComponent<Button>().onClick.AddListener(PauseGame);
            tickTime = ctx.assetModule.sysSettings.gameSystemTickTime;
        }

        void InitSystem()
        {
            moveSystem = new MoveSystem(ctx);
            clearLineSystem = new ClearLineSystem(ctx);
            missionController = new MissionController(ctx);
            scoreSystem = new ScoreSystem(ctx);
            renderSystem = new RenderSystem(ctx);
            vfxSystem = new VFXSystem(ctx);
            itemSystem = new ItemSystem(ctx);
            audioManager = new AudioManager(ctx);
        }
        
        void SubscribeEvents()
        {
            ctx.eventBus.Subscribe<PreviewShapeChangeEvent>(renderSystem.OnResetShapePreview);
            ctx.eventBus.Subscribe<SquareClearedEvent>(vfxSystem.OnLineCleared);
            ctx.eventBus.Subscribe<GameOverEvent>(OnGameOver);
            ctx.eventBus.Subscribe<ReInitGameEvent>(OnRestartGame);
            ctx.eventBus.Subscribe<ReturnMenuEvent>(OnReturnMenu);
            ctx.eventBus.Subscribe<AddItemEvent>(itemSystem.OnGenerateItem);
            ctx.eventBus.Subscribe<ItemBarChangeEvent>(renderSystem.OnItemBarChanged);
            ctx.eventBus.Subscribe<ItemEffectEvent>(itemSystem.OnPositiveItemEffect);
            ctx.eventBus.Subscribe<ItemEffectEvent>(itemSystem.OnNegativeItemEffect);
            ctx.eventBus.Subscribe<StartGameEvent>(OnStartGame);
            ctx.eventBus.Subscribe<ExitGameEvent>(OnExitGame);
            ctx.eventBus.Subscribe<MissionSetEvent>(missionController.OnSetMissionLevel);
            ctx.eventBus.Subscribe<VolumeChangeEvent>(audioManager.OnSetVolume);
            ctx.eventBus.Subscribe<BackGroundMusicSwitchEvent>(audioManager.OnBackGroundMusicSwitch);
            
        }

        public void InitGame()
        {
            // 设置初始任务等级为1
            missionController.SetCurrentMission(1);
            renderSystem.ShowStartMenu();
            // 初始化音频系统
            InitializeAudioSystem();
        }

        public void OnStartGame(StartGameEvent e)
        {
            renderSystem.HideStartMenu();
            ctx.gameSystemState.isRunning = true;
        }

        public void OnExitGame(ExitGameEvent e)
        {
            Application.Quit();
        }
        
        void InitializeAudioSystem()
        {
            if (ctx.assetModule.audioSourcePrefab != null && ctx.audioSource == null)
            {
                // 创建音频对象
                var audioObject = UnityEngine.Object.Instantiate(ctx.assetModule.audioSourcePrefab);
                audioObject.name = "AudioManager";
                var audioSource = audioObject.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    ctx.audioSource = audioSource;
                    audioManager.SetAudioSource(audioSource);
                    // 播放背景音乐
                    audioManager.PlayBackgroundMusic(PGC.Enum.AudiosEnum.BackMusic);
                }
            }
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
            missionController.UpdateMission();
            ctx.inputModule.Update(Time.deltaTime);
            clearLineSystem.ClearWaitForClearModel();
            scoreSystem.ScoreClearSquare();
            ctx.squarePool.ReturnSquareByIndexes();
            RenderSystem.ReRenderSquarePosInGrid(ctx);
            RenderSystem.ReRenderShapePos(ctx);
        }

        public void PauseGame()
        {
            ctx.gameSystemState.isRunning = false;
            renderSystem.ShowPausePopup();
            // audioManager.PauseAudio();
        }
        
        
        public void OnGameOver(GameOverEvent e)
        {
            ctx.gameSystemState.isRunning = false;
            renderSystem.ShowGameOverPopup();
            audioManager.StopAudio();
        }

        void OnReturnMenu(ReturnMenuEvent e)
        {
            Debug.Log("todo OnReturnMenu");
            ResetGameData();
            renderSystem.ShowStartMenu();
        }
        
        void OnRestartGame(ReInitGameEvent e)
        {
            RestartGame();
        }
        
        public void RestartGame()
        {
            //todo data init
            ResetGameData();
            ctx.gameSystemState.isRunning = true;
            // 重新初始化音频系统
            InitializeAudioSystem();
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
            ctx.currentSquareShape.ClearSquares();
            ctx.newReachedSquare.Clear();
            ctx.squaresWaitForDestroy.squares.Clear();
            ctx.currentShapeIsReachedBottom = false;
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