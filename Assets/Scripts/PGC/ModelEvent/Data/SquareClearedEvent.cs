using System.Collections.Generic;
using UnityEngine;

namespace PGC.ModelEvent.Data
{
    public class SquareClearedEvent
    {

        public List<Vector2Int> indexes;

        public SquareClearedEvent(List<Vector2Int> indexes)
        {
            this.indexes = indexes;
        }
    }
}