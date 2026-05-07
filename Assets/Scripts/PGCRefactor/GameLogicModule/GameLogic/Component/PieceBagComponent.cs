using System;
using PGCRefactor.GameLogicModule.GameLogic.Enum;

namespace PGCRefactor.GameLogicModule.GameLogic.Component
{
    public class PieceBagComponent
    {
        public TetrominoType[] bag      = new TetrominoType[7];
        public int             bagIndex = 7; // 7 = exhausted, triggers refill on next draw
        public TetrominoType   previewType;
        public Random   rng      = new ();
    }
}
