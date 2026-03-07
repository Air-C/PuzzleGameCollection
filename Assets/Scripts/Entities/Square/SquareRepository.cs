using System;
using System.Collections.Generic;

namespace PGC {

    public class SquareRepository {

        Dictionary<int, SquareEntity> all;
        Dictionary<Int2, SquareEntity> gridIndexMap;

        public SquareRepository() {
            all = new Dictionary<int, SquareEntity>();
            gridIndexMap = new Dictionary<Int2, SquareEntity>();
        }

        public void Add(SquareEntity square) {
            all.Add(square.id, square);
            gridIndexMap.Add(square.GridIndex, square);
        }

        public bool TryGet(int id, out SquareEntity square) {
            return all.TryGetValue(id, out square);
        }

        public bool TryGetByIndex(Int2 gridIndex, out SquareEntity square) {
            return gridIndexMap.TryGetValue(gridIndex, out square);
        }

        public void RemoveByID(int id) {
            if (all.TryGetValue(id, out var square)) {
                gridIndexMap.Remove(square.GridIndex);
            }
            all.Remove(id);
        }

        public void Remove(SquareEntity square) {
            RemoveByID(square.id);
        }

    }
}