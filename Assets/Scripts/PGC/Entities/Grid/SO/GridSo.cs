using UnityEngine;

namespace PGC.Entities.Grid.SO
{
    [CreateAssetMenu(fileName = "GridSo", menuName = "PGC/GridSO")]
    public class GridSo : ScriptableObject
    {
        public  Vector3 initWorldPosition = new(-2.25f, -4.5f, 0);
        public  int gridWidth = 10;
        public  int gridHeight = 14;
        public  Vector2Int gridTopCenter = new (4, 14);
        public float gridWorldWidth = 0.5f;

        public int PreHeight = 3;
    }
}