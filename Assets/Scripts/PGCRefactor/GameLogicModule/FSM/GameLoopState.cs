using PGCRefactor.GameLogicModule.FSM.Interface;
using PGCRefactor.GameLogicModule.GameLogic.Controller;

namespace PGCRefactor.GameLogicModule.FSM
{
    public class GameLoopState : IState
    {
        private readonly IStateMachine   _stateMachine;
        private readonly GameSessionContext _ctx;

        public GameLoopState(IStateMachine stateMachine, GameSessionContext ctx)
        {
            _stateMachine = stateMachine;
            _ctx          = ctx;
        }

        public void Enter()
        {
            // boardEntity == null means fresh start or post-Restart; skip when resuming from Pause
            if (_ctx.boardEntity == null)
            {
                BoardController.InitBoard(_ctx);
                TetrominoController.PreGenerate(_ctx);
            }
        }

        public void Update()
        {
            if (_ctx.tetrominoEntity.currentCells == null)
            {
                TetrominoController.PrepareFromPreview(_ctx);
                BoardController.SpawnShape(_ctx);
            }
        }

        public void Exit() { }
    }
}