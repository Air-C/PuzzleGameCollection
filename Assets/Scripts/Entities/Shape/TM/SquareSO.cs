using System;
using UnityEngine;
using Custom.Tool;

namespace PGC {

    [CreateAssetMenu(fileName = "So_Shape_", menuName = "PGC/ShapeSO", order = 1)]    
    public class ShapeSO : ScriptableObject {

        public ShapeType shapeType;
        public Int2[] squareIndex;

    }

}