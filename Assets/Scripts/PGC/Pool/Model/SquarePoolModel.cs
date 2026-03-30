using System.Collections.Generic;
using Entities;
using PGC.Enum;

namespace PGC.Pool.Model
{
    public class SquarePoolModel
    {
        public SquareColorEnum squareColor;
        public Queue<SquareEntity> squarePool;

        public SquarePoolModel()
        {
            squarePool = new Queue<SquareEntity>();
        }
    }
}