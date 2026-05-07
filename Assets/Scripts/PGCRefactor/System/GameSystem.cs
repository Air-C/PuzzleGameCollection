using PGCRefactor.GameLogicModule;
using PGCRefactor.GameLogicModule.FSM;
using PGCRefactor.GameLogicModule.FSM.Enum;
using PGCRefactor.GameLogicModule.GameLogic.Controller;
using PGCRefactor.GameLogicModule.GameLogic.TM;
using PGCRefactor.InputModule.Interface;
using UnityEngine;

namespace PGCRefactor.System
{
    public class GameSystem
    {
        private readonly GameFsm            _gameFsm;
        private readonly GameSessionContext _session;

        public GameSystem(GameFsm fsm, BoardSO boardSO, ShapeSO shapeSO, GameSettingSO settingSO, IInputProvider inputProvider)
        {
            _gameFsm = fsm;
            _session = new GameSessionContext
            {
                boardSO       = boardSO,
                shapeSO       = shapeSO,
                settingSO     = settingSO,
                inputProvider = inputProvider
            };
            _gameFsm.SetSessionContext(_session);   // inject once; valid for the whole app lifetime
        }

        // Drives the FSM from Loading state on app start
        public void InitializeGame() => _gameFsm.ChangeState(GameStateEnum.Loading);

        // Drives the FSM Update every frame (called from MonoBehaviour.Update)
        public void GameLoop()
        {
            _session.inputProvider.Update(Time.deltaTime);
            _gameFsm.Update();
        }

        // ---- External state-change triggers (UI buttons) ----

        // Pause: preserve board state, switch to Pause state
        public void GamePause() => _gameFsm.ChangeState(GameStateEnum.Pause);

        // Resume: board entity is non-null, GameLoopState.Enter() skips re-init
        public void Resume() => _gameFsm.ChangeState(GameStateEnum.GameLoop);

        // Restart: wipe session data so GameLoopState.Enter() reinitialises from scratch
        public void Restart()
        {
            _session.boardEntity     = null;
            _session.tetrominoEntity = null;
            _session.pieceBag        = null;
            _session.holdComponent   = null;
            _session.dasComponent    = null;
            _gameFsm.ChangeState(GameStateEnum.GameLoop);
        }

        // GameOver is normally triggered internally by GameLoopState via IStateMachine,
        // but exposed here for external callers (e.g. admin tools, tests).
        public void GameOver() => _gameFsm.ChangeState(GameStateEnum.GameOver);

        // ---- Shape generation API (for external callers / UI) ----

        public void PreGenerate()      => TetrominoController.PreGenerate(_session);
        public void RefreshPreview()   => TetrominoController.RefreshPreview(_session);
        public void SpawnFromPreview()
        {
            TetrominoController.PrepareFromPreview(_session);
            BoardController.SpawnShape(_session);
        }
    }
}
