using PGCRefactor.GameLogicModule.GameLogic.Component;
using PGCRefactor.GameLogicModule.GameLogic.Entity;
using PGCRefactor.GameLogicModule.GameLogic.TM;

namespace PGCRefactor.GameLogicModule
{
    public class GameSessionContext
    {
        // ---- Board ----
        public BoardSO    boardSO;
        public BoardEntity boardEntity;

        // ---- Shape config ----
        public ShapeSO shapeSO;

        // ---- Tetromino ----
        public TetrominoEntity   tetrominoEntity;
        public PieceBagComponent pieceBag;
    }
}
