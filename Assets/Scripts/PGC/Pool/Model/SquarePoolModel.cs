using System.Collections.Generic;
using Entities;
using PGC.Enum;

namespace PGC.Pool.Model
{
    public class SquarePoolModel
    {
        public string squareName;
        public Queue<SquareEntity> squarePool;

        public SquarePoolModel()
        {
            squarePool = new Queue<SquareEntity>();
        }
    }
}