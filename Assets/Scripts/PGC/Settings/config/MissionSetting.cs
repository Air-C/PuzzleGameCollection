using System;

namespace PGC.System
{
    [Serializable]
    public struct MissionSetting
    {
        public int missionID;
        public float moveSystemMoveInterval;
        public float moveSystemAutoMoveInterval;
        public float horizontalMoveDelay;
        public float specialItemHitRate;
    }
}