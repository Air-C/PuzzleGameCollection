using System.Collections.Generic;
using PGC.Enum;
using UnityEngine;

namespace Entities.SO
{
    [CreateAssetMenu(menuName = "PGC/SquareShapeSo", fileName = "SquareShapeSo")]
    public class SquareShapeSo : ScriptableObject
    {
        public ShapeTypeEnum type;
        // public GameObject prefab;
        public List<Vector2Int> offsets;
    }
}