using System;
using System.Collections.Generic;
using UnityEngine;

namespace PGC {

    public class ShapeEntity {

        public HashSet<int> squareIds;

        public ShapeEntity() {
            squareIds = new HashSet<int>();
        }

        public void AddSquare(int squareId) {
            squareIds.Add(squareId);
        }

        public ICollection<int> GetSquareIds() {
            return squareIds;
        }

    }

}