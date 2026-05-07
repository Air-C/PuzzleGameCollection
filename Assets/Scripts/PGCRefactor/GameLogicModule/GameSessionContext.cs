using PGCRefactor.GameLogicModule.GameLogic.Component;
using PGCRefactor.GameLogicModule.GameLogic.Entity;
using PGCRefactor.GameLogicModule.GameLogic.TM;
using PGCRefactor.InputModule.Interface;

namespace PGCRefactor.GameLogicModule
{
    public class GameSessionContext
    {
        // ---- Board ----
        public BoardSO     boardSO;
        public BoardEntity boardEntity;

        // ---- Shape config ----
        public ShapeSO shapeSO;

        // ---- Game setting ----
        public GameSettingSO settingSO;

        // ---- Tetromino ----
        public TetrominoEntity   tetrominoEntity;
        public PieceBagComponent pieceBag;

        // ---- Input ----
        public IInputProvider inputProvider;

        // ---- Movement ----
        public HoldComponent holdComponent;
        public DASComponent  dasComponent;
    }
}
