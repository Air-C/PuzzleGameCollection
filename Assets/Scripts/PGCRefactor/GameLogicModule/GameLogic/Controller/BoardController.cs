using PGCRefactor.GameLogicModule.GameLogic.Entity;
using PGCRefactor.GameLogicModule.GameLogic.Enum;
using PGCRefactor.GameLogicModule.GameLogic.System;
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

        // Stamps the current piece's cells as Occupied (locking it to the board) and resets the entity.
        // currentCells must still hold the last valid positions; they may already be Free on the grid
        // (e.g. cleared by TryFall before this call).
        public static void LockPiece(GameSessionContext ctx)
        {
            var grid = ctx.boardEntity.grid;
            foreach (var cell in ctx.tetrominoEntity.currentCells)
                grid.cells[cell.x, cell.y] = CellState.Occupied;
            ctx.tetrominoEntity.Reset();
        }

        // Clears the active piece's TempOccupied cells from the board (without resetting the entity).
        // Used by GameLoopState when orchestrating Hold before spawning a replacement piece.
        public static void ClearActiveCells(GameSessionContext ctx)
        {
            var grid = ctx.boardEntity.grid;
            foreach (var cell in ctx.tetrominoEntity.currentCells)
                grid.cells[cell.x, cell.y] = CellState.Free;
        }

        // Gravity drop: delegates the one-row attempt to PieceSystem (shared primitive),
        // then locks the piece and runs line-clear if the row below is blocked.
        public static void TryFall(GameSessionContext ctx)
        {
            if (!PieceSystem.TryDropOneRow(ctx))
                LockAndClearLines(ctx);
        }

        // Reusable buffers — no per-lock allocation.
        private static readonly int[] _affectedRows = new int[4];
        private static readonly int[] _fullRows      = new int[4];

        // Locks the piece, detects full rows among only the affected rows,
        // then shifts the board in a single pass.
        private static void LockAndClearLines(GameSessionContext ctx)
        {
            var te   = ctx.tetrominoEntity;
            var grid = ctx.boardEntity.grid;

            // --- Collect unique affected Y values ---
            int affectedCount = 0;
            foreach (var cell in te.currentCells)
            {
                bool found = false;
                for (int i = 0; i < affectedCount; i++)
                    if (_affectedRows[i] == cell.y) { found = true; break; }
                if (!found) _affectedRows[affectedCount++] = cell.y;
            }

            // --- Stamp Occupied ---
            foreach (var cell in te.currentCells)
                grid.cells[cell.x, cell.y] = CellState.Occupied;

            // --- Phase 1: detect all full rows (only check affected rows) ---
            int fullCount = 0;
            for (int i = 0; i < affectedCount; i++)
            {
                int y = _affectedRows[i];
                bool full = true;
                for (int x = 0; x < grid.width; x++)
                    if (grid.cells[x, y] != CellState.Occupied) { full = false; break; }
                if (full) _fullRows[fullCount++] = y;
            }

            // --- Phase 2: shift all rows above cleared lines down ---
            if (fullCount > 0)
                ShiftRowsDown(ctx, _fullRows, fullCount);

            te.Reset();
        }

        // Shifts rows above cleared lines downward, preserving existing holes.
        // Accepts the cleared-row indices so any clearing strategy can reuse it.
        // y=0 is the floor; rows above a cleared line (higher y) shift down (lower y).
        public static void ShiftRowsDown(GameSessionContext ctx, int[] fullRows, int fullCount)
        {
            var grid = ctx.boardEntity.grid;

            int startY = fullRows[0];
            for (int i = 1; i < fullCount; i++)
                if (fullRows[i] < startY) startY = fullRows[i];

            int writeY = startY;
            for (int readY = startY; readY < grid.height; readY++)
            {
                bool isFull = false;
                for (int i = 0; i < fullCount; i++)
                    if (fullRows[i] == readY) { isFull = true; break; }
                if (isFull) continue;

                if (readY != writeY)
                    for (int x = 0; x < grid.width; x++)
                        grid.cells[x, writeY] = grid.cells[x, readY];
                writeY++;
            }

            for (int y = writeY; y < grid.height; y++)
                for (int x = 0; x < grid.width; x++)
                    grid.cells[x, y] = CellState.Free;
        }

        // Compresses each column independently: occupied cells sink to the bottom (y=0),
        // Free gaps bubble to the top. For special clearing strategies only —
        // standard line-clear uses ShiftRowsDown to preserve existing holes.
        public static void CompressColumns(GameSessionContext ctx)
        {
            var grid   = ctx.boardEntity.grid;
            int height = grid.height;

            for (int x = 0; x < grid.width; x++)
            {
                int writeY = 0;
                for (int readY = 0; readY < height; readY++)
                {
                    if (grid.cells[x, readY] == CellState.Free) continue;
                    grid.cells[x, writeY] = grid.cells[x, readY];
                    if (writeY != readY) grid.cells[x, readY] = CellState.Free;
                    writeY++;
                }
                for (int y = writeY; y < height; y++)
                    grid.cells[x, y] = CellState.Free;
            }
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
