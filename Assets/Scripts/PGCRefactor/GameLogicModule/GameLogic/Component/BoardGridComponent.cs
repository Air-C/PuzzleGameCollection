using PGCRefactor.GameLogicModule.GameLogic.Enum;

namespace PGCRefactor.GameLogicModule.GameLogic.Component
{
    public class BoardGridComponent
    {
        public readonly int width;
        public readonly int height;
        public CellState[,] cells;

        public BoardGridComponent(int width, int height)
        {
            this.width  = width;
            this.height = height;
            cells = new CellState[width, height];
        }
    }
}
