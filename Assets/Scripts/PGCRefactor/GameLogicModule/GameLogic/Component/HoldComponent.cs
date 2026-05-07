using PGCRefactor.GameLogicModule.GameLogic.Enum;

namespace PGCRefactor.GameLogicModule.GameLogic.Component
{
    public class HoldComponent
    {
        public TetrominoType heldType;
        public bool          hasPiece;  // whether a piece is stored in the hold slot
        public bool          isLocked;  // hold already used this spawn; resets when next piece appears
    }
}
