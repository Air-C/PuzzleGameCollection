using PGCRefactor.GameLogicModule.GameLogic.Component;
using PGCRefactor.GameLogicModule.GameLogic.Enum;
using PGCRefactor.GameLogicModule.GameLogic.System;
using PGCRefactor.GameLogicModule.GameLogic.TM;
using PGCRefactor.AssetsLoadModule.InputModule.Enum;
using UnityEngine;

namespace PGCRefactor.GameLogicModule.GameLogic.Controller
{
    public static class MoveController
    {
        // Reusable 4-element buffers; all tetrominos have exactly 4 cells.
        // _rotateBuffer: written in RotateCW, stored as te.offsets after a successful rotation.
        //   Safe to alias (offsets[i] read before result[i] written in same iteration).
        // _cellBuffer: written in StampCells, stored as te.currentCells.
        //   Safe to alias because ClearCurrentCells finishes reading before StampCells overwrites.
        private static readonly Vector2Int[] _rotateBuffer = new Vector2Int[4];
        private static readonly Vector2Int[] _cellBuffer   = new Vector2Int[4];

        // Entry point — call every frame while an active piece exists.
        public static void ProcessInput(GameSessionContext ctx, float dt)
        {
            var input = ctx.inputProvider;

            HandleHorizontal(ctx, dt);

            if (input.VerticalMove.value != 0)
                TryMoveDown(ctx);

            if (input.Rotate.status == InputStatus.Pressed)
                TryRotateCW(ctx);

            input.Consume();
        }

        // ---- Movement ----

        private static void HandleHorizontal(GameSessionContext ctx, float dt)
        {
            var input = ctx.inputProvider;
            var das   = ctx.dasComponent;
            int hDir  = input.HorizontalMove.value;

            if (hDir == 0)
            {
                das.holdTimer    = 0f;
                das.arrTimer     = 0f;
                das.lastDirection = 0;
                return;
            }

            // First press or direction flip: move immediately, reset DAS timers.
            if (input.HorizontalMove.status == InputStatus.Pressed || hDir != das.lastDirection)
            {
                TryMoveHorizontal(ctx, hDir);
                das.holdTimer    = 0f;
                das.arrTimer     = 0f;
                das.lastDirection = hDir;
                return;
            }

            // Key held: wait for DAS delay, then auto-repeat at ARR rate.
            das.holdTimer += dt;
            if (das.holdTimer < DASComponent.DasDelay) return;

            das.arrTimer += dt;
            if (das.arrTimer < DASComponent.ArrRate) return;

            TryMoveHorizontal(ctx, hDir);
            das.arrTimer = 0f;
        }

        private static void TryMoveHorizontal(GameSessionContext ctx, int dir)
        {
            var te       = ctx.tetrominoEntity;
            var newPivot = new Vector2Int(te.pivot.x + dir, te.pivot.y);

            ClearCurrentCells(ctx);
            if (IsValid(ctx, newPivot, te.offsets))
                StampCells(ctx, newPivot, te.offsets);
            else
                StampCells(ctx, te.pivot, te.offsets);
        }

        private static void TryMoveDown(GameSessionContext ctx)
        {
            PieceSystem.TryDropOneRow(ctx);
        }

        // ---- SRS Rotation ----

        private static void TryRotateCW(GameSessionContext ctx)
        {
            var te         = ctx.tetrominoEntity;
            var newOffsets = RotateCW(te.offsets);
            var newState   = (te.rotationState + 1) % 4;
            var kicks      = SRSData.GetKicks(te.type, te.rotationState, 1);

            ClearCurrentCells(ctx);
            foreach (var kick in kicks)
            {
                var newPivot = te.pivot + kick;
                if (!IsValid(ctx, newPivot, newOffsets)) continue;

                te.offsets       = newOffsets;
                te.rotationState = newState;
                StampCells(ctx, newPivot, newOffsets);
                return;
            }
            // All kick tests failed — restore original position.
            StampCells(ctx, te.pivot, te.offsets);
        }

        // CW rotation in y-up display: (x, y) → (y, -x)
        private static Vector2Int[] RotateCW(Vector2Int[] offsets)
        {
            for (int i = 0; i < offsets.Length; i++)
                _rotateBuffer[i] = new Vector2Int(offsets[i].y, -offsets[i].x);
            return _rotateBuffer;
        }

        // ---- Board helpers ----

        private static bool IsValid(GameSessionContext ctx, Vector2Int pivot, Vector2Int[] offsets)
        {
            var grid = ctx.boardEntity.grid;
            foreach (var offset in offsets)
            {
                int x = pivot.x + offset.x;
                int y = pivot.y + offset.y;
                if (x < 0 || x >= grid.width)  return false;
                if (y < 0 || y >= grid.height) return false;
                if (grid.cells[x, y] == CellState.Occupied) return false;
            }
            return true;
        }

        private static void ClearCurrentCells(GameSessionContext ctx)
        {
            var grid = ctx.boardEntity.grid;
            foreach (var cell in ctx.tetrominoEntity.currentCells)
                grid.cells[cell.x, cell.y] = CellState.Free;
        }

        private static void StampCells(GameSessionContext ctx, Vector2Int pivot, Vector2Int[] offsets)
        {
            var te   = ctx.tetrominoEntity;
            var grid = ctx.boardEntity.grid;

            for (int i = 0; i < offsets.Length; i++)
                _cellBuffer[i] = pivot + offsets[i];

            foreach (var cell in _cellBuffer)
                grid.cells[cell.x, cell.y] = CellState.TempOccupied;

            te.pivot        = pivot;
            te.currentCells = _cellBuffer;
        }
    }
}
