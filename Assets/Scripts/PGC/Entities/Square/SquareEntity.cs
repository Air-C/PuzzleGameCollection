using UnityEngine;

namespace Entities
{
    public class SquareEntity
    {

        public int X { get; set; }
        public int Y { get; set; }
        public GameObject SquareObj {get; set; }
        
        public SquareEntity(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void RestIndex(int x, int y)
        {
            X = x;
            Y = y;
        }
        
    }
}