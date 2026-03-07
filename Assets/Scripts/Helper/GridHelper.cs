using System;
using UnityEngine;
using Custom.Tool;

namespace PGC {

    public static class GridHelper {

        static Vector3 initPos = new(-2.5f, -4.5f, 0);
        static float gridWidth = 0.5f;

        public static Vector3 GetSquareWorldPos(Int2 squareIndex) {
            return new Vector3(squareIndex.x * gridWidth, squareIndex.y * gridWidth, 0) + initPos;
        }

    }
}