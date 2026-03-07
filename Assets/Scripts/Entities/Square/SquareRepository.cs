using System;
using System.Collections.Generic;

namespace PGC {

    public class SquareRepository {

        Dictionary<int, SquareEntity> all;

        public SquareRepository() {
            all = new Dictionary<int, SquareEntity>();
        }

        public void Add(SquareEntity square) {
            all.Add(square.id, square);
        }

        public bool TryGet(int id, out SquareEntity square) {
            return all.TryGetValue(id, out square);
        }

        public void RemoveByID(int id) {
            all.Remove(id);
        }

        public void Remove(SquareEntity square) {
            all.Remove(square.id);
        }

    }
}