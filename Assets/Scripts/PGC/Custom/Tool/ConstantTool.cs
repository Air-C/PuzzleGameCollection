using Entities;
using Unity.Mathematics;
using UnityEngine;
using MoveDirection = PGC.Enum.MoveDirection;
using Random = System.Random;

namespace Custom.Tool
{
    public class ConstantTool
    {

        public static int GetRandomInt(int min, int max)
        {
            Random rand = new Random();
            return rand.Next(min, max);
        }
        
        public static Vector2Int GetNewIndexOfHorizontalMove(int x, int y, int moveValue)
        {
            Vector2Int newIndex = new (x + moveValue, y);
            return newIndex;
        }
        
        public static Vector2Int GetNewIndexOfVerticalMove(int x, int y, int moveValue)
        {
            Vector2Int newIndex = new (x, y + moveValue);
            return newIndex;
        }

        public static Vector2Int GetNewIndexOfRotate(int x, int y, SquareEntity pivotSquare)
        {
            Vector2Int offsetIndex = new Vector2Int(x - pivotSquare.X, y - pivotSquare.Y);
            return new (pivotSquare.X + offsetIndex.y, pivotSquare.Y - offsetIndex.x);
        }

        public static Vector3 GetSquareWorldPos(SquareEntity square, float width)
        {
            return new Vector3(square.X * width, square.Y * width, 0 );
        }
        
        public static bool Hit(float probability)
        {
            return UnityEngine.Random.value < probability;
        }
    }

}