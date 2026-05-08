using PGCRefactor.GameLogicModule.GameLogic.Component;
using PGCRefactor.GameLogicModule.GameLogic.TM;

namespace PGCRefactor.GameLogicModule.GameLogic.Entity
{
    public class BoardEntity
    {
        public BoardGridComponent grid;

        public BoardEntity(BoardSO boardSO)
        {
            grid = new BoardGridComponent(boardSO.width, boardSO.height + boardSO.preHeight);
        }
    }
}
