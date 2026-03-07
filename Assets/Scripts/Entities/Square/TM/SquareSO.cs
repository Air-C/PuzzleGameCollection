using System;
using UnityEngine;
using Custom.Tool;

namespace PGC {

    [CreateAssetMenu(fileName = "So_Square_", menuName = "PGC/SquareSO", order = 1)]    
    public class SquareSO : ScriptableObject {

        public ShapeType shapeType;
        public Int2[] squareIndex;

    }

}