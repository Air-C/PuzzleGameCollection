using PGCRefactor.GameLogicModule.GameLogic.Enum;
using UnityEngine;

namespace PGCRefactor.GameLogicModule.GameLogic.TM
{
    // Wall-kick tables for Super Rotation System.
    // Coordinate system: y-down (y increases toward bottom of board).
    // Values are derived from the Tetris guideline tables (y-up) with y negated.
    //
    // Index = fromState * 2 + (isCW ? 0 : 1)
    //   0: 0→1 CW    1: 0→3 CCW
    //   2: 1→2 CW    3: 1→0 CCW
    //   4: 2→3 CW    5: 2→1 CCW
    //   6: 3→0 CW    7: 3→2 CCW
    public static class SRSData
    {
        // JLSTZ pieces share one kick table.
        private static readonly Vector2Int[][] JLSTZKicks =
        {
            // 0→1 CW
            new[] { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,+2), new Vector2Int(-1,+2) },
            // 0→3 CCW
            new[] { new Vector2Int(0,0), new Vector2Int(+1,0), new Vector2Int(+1,-1), new Vector2Int(0,+2), new Vector2Int(+1,+2) },
            // 1→2 CW
            new[] { new Vector2Int(0,0), new Vector2Int(+1,0), new Vector2Int(+1,+1), new Vector2Int(0,-2), new Vector2Int(+1,-2) },
            // 1→0 CCW
            new[] { new Vector2Int(0,0), new Vector2Int(+1,0), new Vector2Int(+1,+1), new Vector2Int(0,-2), new Vector2Int(+1,-2) },
            // 2→3 CW
            new[] { new Vector2Int(0,0), new Vector2Int(+1,0), new Vector2Int(+1,-1), new Vector2Int(0,+2), new Vector2Int(+1,+2) },
            // 2→1 CCW
            new[] { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,-1), new Vector2Int(0,+2), new Vector2Int(-1,+2) },
            // 3→0 CW
            new[] { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,+1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
            // 3→2 CCW
            new[] { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(-1,+1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        };

        // I piece has its own kick table.
        private static readonly Vector2Int[][] IKicks =
        {
            // 0→1 CW
            new[] { new Vector2Int(0,0), new Vector2Int(-2,0), new Vector2Int(+1,0), new Vector2Int(-2,+1), new Vector2Int(+1,-2) },
            // 0→3 CCW
            new[] { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(+2,0), new Vector2Int(-1,-2), new Vector2Int(+2,+1) },
            // 1→2 CW
            new[] { new Vector2Int(0,0), new Vector2Int(-1,0), new Vector2Int(+2,0), new Vector2Int(-1,-2), new Vector2Int(+2,+1) },
            // 1→0 CCW
            new[] { new Vector2Int(0,0), new Vector2Int(+2,0), new Vector2Int(-1,0), new Vector2Int(+2,-1), new Vector2Int(-1,+2) },
            // 2→3 CW
            new[] { new Vector2Int(0,0), new Vector2Int(+2,0), new Vector2Int(-1,0), new Vector2Int(+2,-1), new Vector2Int(-1,+2) },
            // 2→1 CCW
            new[] { new Vector2Int(0,0), new Vector2Int(+1,0), new Vector2Int(-2,0), new Vector2Int(+1,+2), new Vector2Int(-2,-1) },
            // 3→0 CW
            new[] { new Vector2Int(0,0), new Vector2Int(+1,0), new Vector2Int(-2,0), new Vector2Int(+1,+2), new Vector2Int(-2,-1) },
            // 3→2 CCW
            new[] { new Vector2Int(0,0), new Vector2Int(-2,0), new Vector2Int(+1,0), new Vector2Int(-2,+1), new Vector2Int(+1,-2) },
        };

        private static readonly Vector2Int[] OKicks = { Vector2Int.zero };

        public static Vector2Int[] GetKicks(TetrominoType type, int fromState, int dir)
        {
            if (type == TetrominoType.O)
                return OKicks;

            int idx = fromState * 2 + (dir > 0 ? 0 : 1);
            return type == TetrominoType.I ? IKicks[idx] : JLSTZKicks[idx];
        }
    }
}
