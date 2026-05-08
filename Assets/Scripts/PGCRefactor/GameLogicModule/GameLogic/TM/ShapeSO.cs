using System;
using PGCRefactor.GameLogicModule.GameLogic.Enum;
using UnityEngine;

namespace PGCRefactor.GameLogicModule.GameLogic.TM
{
    [CreateAssetMenu(fileName = "So_Shape_Default", menuName = "PGC/Shape SO")]
    public class ShapeSO : ScriptableObject
    {
        // indexed by (int)TetrominoType — I=0 through L=6
        public ShapeData[] shapes;

        public Vector2Int[] GetOffsets(TetrominoType type) => shapes[(int)type].offsets;
    }

    [Serializable]
    public class ShapeData
    {
        public TetrominoType type;

        // all cell offsets relative to pivot, including the pivot cell itself (0,0)
        // y-up: positive y = above pivot. Pivot sits on the bottom row of the piece.
        // I : (-1,0)(0,0)(1,0)(2,0)
        // O : (0,0)(1,0)(0,1)(1,1)
        // T : (-1,0)(0,0)(1,0)(0,1)
        // S : (-1,0)(0,0)(0,1)(1,1)
        // Z : (0,0)(1,0)(-1,1)(0,1)
        // J : (-1,0)(0,0)(1,0)(-1,1)
        // L : (-1,0)(0,0)(1,0)(1,1)
        public Vector2Int[] offsets;
    }
}
