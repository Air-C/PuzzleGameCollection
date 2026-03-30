using System;

namespace PGC.System
{
    public enum SpecialItemMode
    {
        Disabled,
        PositiveOnly,
        NegativeOnly,
        Both,
    }

    [Serializable]
    public struct MissionSetting
    {
        public int missionID;
        public float moveSystemMoveInterval;
        public float moveSystemAutoMoveInterval;
        public float horizontalMoveDelay;
        public float specialItemHitRate;
        public bool enableSpecialItems;
        public SpecialItemMode specialItemMode;
    }
}