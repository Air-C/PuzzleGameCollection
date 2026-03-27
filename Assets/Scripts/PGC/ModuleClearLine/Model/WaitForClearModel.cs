using System.Collections.Generic;
using PGC.Enum;
using UnityEngine;

namespace PGC.ModuleClearLine.Model
{
    public struct WaitForClearModel
    {
        public ClearSquareType clearSquareType;
        public HashSet<int> waitForClearRows;
        public HashSet<int> waitForClearColumns;
        public (Vector2Int index, int radius) waitForClearCircle;
        
    }
}