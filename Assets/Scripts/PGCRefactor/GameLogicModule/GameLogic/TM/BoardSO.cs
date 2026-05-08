using UnityEngine;

namespace PGCRefactor.GameLogicModule.GameLogic.TM
{
    [CreateAssetMenu(fileName = "So_Board_Default", menuName = "PGC/Board SO")]
    public class BoardSO : ScriptableObject
    {
        public int width  = 10;
        public int height = 14;
        public  Vector2Int initCenter = new (4, 14);
        public int preHeight = 3;

    }
}
