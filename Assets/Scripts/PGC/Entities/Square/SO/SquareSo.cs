
using System.Collections.Generic;
using PGC.Enum;
using UnityEngine;

namespace Entities.SO
{
    [CreateAssetMenu(menuName = "PGC/SquareSo", fileName = "SquareSo")]
    public class SquareSo : ScriptableObject
    {
        public SquareColorEnum color;
        public GameObject prefab;
        
    }
}