using System.Collections.Generic;
using Entities.SO;
using PGC;
using UnityEngine;

namespace Entities
{
    public class SquareShapeEntity
    {
        private List<SquareEntity> squares = new ();
        public string Name{get;set;}
        

        public void AddSquare(SquareEntity square)
        {
            squares.Add(square);
        }

        public void AddSquaresFirst(SquareEntity square)
        {
            squares.Insert(0,square);
        }

        public List<SquareEntity> GetSquares()
        {
            return squares;
        }

        public void ClearSquares()
        {
            squares.Clear();
        }
    }
}