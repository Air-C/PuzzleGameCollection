using PGCRefactor.GameLogicModule.GameLogic.Component;
using PGCRefactor.GameLogicModule.GameLogic.Enum;
using UnityEngine;

namespace PGCRefactor.GameLogicModule.GameLogic.Controller
{
    public static class TetrominoController
    {
        // Draw the next TetrominoType from the bag; refills via Fisher-Yates when exhausted.
        private static TetrominoType DrawNext(PieceBagComponent bag)
        {
            if (bag.bagIndex >= 7)
            {
                for (int i = 0; i < 7; i++) bag.bag[i] = (TetrominoType)i;
                for (int i = 6; i > 0; i--)
                {
                    int j = bag.rng.Next(i + 1);
                    (bag.bag[i], bag.bag[j]) = (bag.bag[j], bag.bag[i]);
                }
                bag.bagIndex = 0;
            }
            return bag.bag[bag.bagIndex++];
        }

        // Initialise the bag and pre-fill the first preview piece.
        public static void PreGenerate(GameSessionContext ctx)
        {
            ctx.pieceBag             = new PieceBagComponent();
            ctx.pieceBag.previewType = DrawNext(ctx.pieceBag);
        }

        // Replace the current preview with the next piece drawn from the bag.
        public static void RefreshPreview(GameSessionContext ctx)
        {
            ctx.pieceBag.previewType = DrawNext(ctx.pieceBag);
        }

        // Populate tetrominoEntity from a specific type (used by Hold mechanic).
        // Does not draw from or refresh the preview queue.
        // Caller must invoke BoardController.SpawnShape(ctx) immediately after.
        public static void PrepareFromType(GameSessionContext ctx, TetrominoType type)
        {
            var pivot = ctx.boardSO.initCenter;
            ctx.tetrominoEntity.Reset();
            ctx.tetrominoEntity.type    = type;
            ctx.tetrominoEntity.pivot   = pivot;
            ctx.tetrominoEntity.offsets = ctx.shapeSO.GetOffsets(type);
        }

        // Populate tetrominoEntity from the current preview and advance the preview queue.
        // Caller must invoke BoardController.SpawnShape(ctx) immediately after to stamp the board.
        public static void PrepareFromPreview(GameSessionContext ctx)
        {
            var type    = ctx.pieceBag.previewType;
            var pivot   = ctx.boardSO.initCenter;
            ctx.tetrominoEntity.Reset();
            ctx.tetrominoEntity.type    = type;
            ctx.tetrominoEntity.pivot   = pivot;
            ctx.tetrominoEntity.offsets = ctx.shapeSO.GetOffsets(type);
            RefreshPreview(ctx);
        }
    }
}
