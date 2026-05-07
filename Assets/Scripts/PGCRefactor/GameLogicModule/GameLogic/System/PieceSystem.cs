using PGCRefactor.GameLogicModule.GameLogic.Enum;
using UnityEngine;

namespace PGCRefactor.GameLogicModule.GameLogic.System
{
    // Shared board-level primitives used by multiple Controllers.
    // No Controller dependencies — Controllers call down into this layer.
    public static class PieceSystem
    {
        private static readonly Vector2Int[] _cellBuffer = new Vector2Int[4];

        // Attempt to move the active piece down one row.
        // Returns true if the piece moved; false if blocked (piece is restored to TempOccupied).
        // Callers decide what to do on failure: soft-drop ignores it, TryFall locks the piece.
        public static bool TryDropOneRow(GameSessionContext ctx)
        {
            var te       = ctx.tetrominoEntity;
            var grid     = ctx.boardEntity.grid;
            var newPivot = new Vector2Int(te.pivot.x, te.pivot.y + 1);

            foreach (var cell in te.currentCells)
                grid.cells[cell.x, cell.y] = CellState.Free;

            bool valid = true;
            foreach (var offset in te.offsets)
            {
                int x = newPivot.x + offset.x, y = newPivot.y + offset.y;
                if (x < 0 || x >= grid.width || y < 0 || y >= grid.height
                    || grid.cells[x, y] == CellState.Occupied)
                { valid = false; break; }
            }

            if (valid)
            {
                for (int i = 0; i < te.offsets.Length; i++)
                    _cellBuffer[i] = newPivot + te.offsets[i];
                foreach (var cell in _cellBuffer)
                    grid.cells[cell.x, cell.y] = CellState.TempOccupied;
                te.pivot        = newPivot;
                te.currentCells = _cellBuffer;
                return true;
            }

            foreach (var cell in te.currentCells)
                grid.cells[cell.x, cell.y] = CellState.TempOccupied;
            return false;
        }
    }
}
