using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;
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

        public static T GetEnumNext<T>(T value)
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            int index = Array.IndexOf(values, value);
            return values[(index+1) % values.Length];
        }
        
        /// <summary>
        /// 使用 Fisher-Yates 算法原地打乱列表顺序
        /// </summary>
        private static readonly Random _globalRandom = new Random();
        public static void Shuffle<T>(IList<T> list)
        {
            if (list == null || list.Count <= 1) return;

            int n = list.Count;
            while (n > 1)
            {
                n--;
                // 生成 [0, n] 范围内的随机索引
                int k = _globalRandom.Next(n + 1);
            
                // 交换元素
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}