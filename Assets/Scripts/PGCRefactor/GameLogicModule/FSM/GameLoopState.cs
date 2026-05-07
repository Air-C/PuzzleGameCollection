using PGCRefactor.GameLogicModule.FSM.Interface;
using PGCRefactor.GameLogicModule.GameLogic.Component;
using PGCRefactor.GameLogicModule.GameLogic.Controller;
using UnityEngine;

namespace PGCRefactor.GameLogicModule.FSM
{
    public class GameLoopState : IState
    {
        private readonly IStateMachine      _stateMachine;
        private readonly GameSessionContext _ctx;

        private float _tickTimer;

        public GameLoopState(IStateMachine stateMachine, GameSessionContext ctx)
        {
            _stateMachine = stateMachine;
            _ctx          = ctx;
        }

        public void Enter()
        {
            _tickTimer = 0f;

            // boardEntity == null means fresh start or post-Restart; skip when resuming from Pause
            if (_ctx.boardEntity == null)
            {
                BoardController.InitBoard(_ctx);
                TetrominoController.PreGenerate(_ctx);
                _ctx.holdComponent = new HoldComponent();
                _ctx.dasComponent  = new DASComponent();
            }
        }

        public void Update()
        {
            if (_ctx.tetrominoEntity.currentCells == null)
            {
                _ctx.holdComponent.isLocked = false;
                TetrominoController.PrepareFromPreview(_ctx);
                BoardController.SpawnShape(_ctx);
            }
            else
            {
                bool changeRequested = _ctx.inputProvider.IsChange;
                bool holdRequested   = _ctx.inputProvider.IsHold;
                MoveController.ProcessInput(_ctx, Time.deltaTime);
                if (changeRequested) TetrominoController.RefreshPreview(_ctx);
                if (holdRequested)   HandleHold();
                Tick(Time.deltaTime);
            }
        }

        // Hold orchestration: coordinates TetrominoController and BoardController.
        // Lives here (orchestration layer) because it spans multiple Controller domains.
        private void HandleHold()
        {
            var hold = _ctx.holdComponent;
            if (hold.isLocked) return;

            var curType = _ctx.tetrominoEntity.type;
            BoardController.ClearActiveCells(_ctx);

            if (!hold.hasPiece)
            {
                hold.heldType = curType;
                hold.hasPiece = true;
                TetrominoController.PrepareFromPreview(_ctx);
            }
            else
            {
                var swapType  = hold.heldType;
                hold.heldType = curType;
                TetrominoController.PrepareFromType(_ctx, swapType);
            }

            BoardController.SpawnShape(_ctx);
            hold.isLocked = true;
        }

        // Executes auto-fall at the fixed interval configured in GameSettingSO.
        private void Tick(float dt)
        {
            _tickTimer += dt;
            if (_tickTimer < _ctx.settingSO.tickInterval) return;
            _tickTimer -= _ctx.settingSO.tickInterval;

            BoardController.TryFall(_ctx);
        }

        public void Exit() { }
    }
}
