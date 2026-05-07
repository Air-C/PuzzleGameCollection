using PGCRefactor.GameLogicModule.GameLogic.Enum;
using UnityEngine;

namespace PGCRefactor.GameLogicModule.GameLogic.Entity
{
    public class TetrominoEntity
    {
        public TetrominoType type;

        // reference position on the board
        public Vector2Int   pivot;

        // all cell offsets relative to pivot, including the pivot cell itself (0,0)
        // sourced from ShapeSO.ShapeData.offsets
        public Vector2Int[] offsets;

        // board positions currently TempOccupied: pivot + offset[i] for each i
        // null when no active piece (triggers next spawn)
        public Vector2Int[] currentCells;

        // Call when the piece locks; resets all fields so the entity is ready for the next spawn.
        public void Reset()
        {
            type         = default;
            pivot        = default;
            offsets      = null;
            currentCells = null;
        }
    }
}
