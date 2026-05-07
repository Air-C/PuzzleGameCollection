using PGCRefactor.GameLogicModule.GameLogic.Entity;
using PGCRefactor.GameLogicModule.GameLogic.Enum;
using UnityEngine;

namespace PGCRefactor.GameLogicModule.GameLogic.Controller
{
    public static class BoardController
    {
        // Creates BoardEntity and a blank TetrominoEntity for the session.
        public static void InitBoard(GameSessionContext ctx)
        {
            ctx.boardEntity     = new BoardEntity(ctx.boardSO);
            ctx.tetrominoEntity = new TetrominoEntity(); // currentCells == null → spawn needed
        }

        // Reads pivot/offsets from ctx.tetrominoEntity (set by TetrominoController.PrepareFromPreview),
        // stamps TempOccupied on the grid, and writes currentCells back.
        public static void SpawnShape(GameSessionContext ctx)
        {
            var te       = ctx.tetrominoEntity;
            var allCells = new Vector2Int[te.offsets.Length];

            for (int i = 0; i < te.offsets.Length; i++)
                allCells[i] = te.pivot + te.offsets[i];

            foreach (var cell in allCells)
                ctx.boardEntity.grid.cells[cell.x, cell.y] = CellState.TempOccupied;

            te.currentCells = allCells;
        }
    }
}
