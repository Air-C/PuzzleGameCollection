using UnityEngine;

namespace PGCRefactor.GameLogicModule.GameLogic.TM
{
    [CreateAssetMenu(fileName = "So_GameSetting_Default", menuName = "PGC/Game Setting SO")]
    public class GameSettingSO : ScriptableObject
    {
        [Tooltip("Seconds between automatic fall steps (lower = faster)")]
        public float tickInterval = 1.0f;
    }
}
