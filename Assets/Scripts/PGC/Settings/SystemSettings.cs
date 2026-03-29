
using System.Collections.Generic;
using PGC.System;
using UnityEngine;

namespace PGC.Settings
{
    [CreateAssetMenu(menuName = "PGC/Settings", fileName = "PGCSettings")]
    public class SystemSettings : ScriptableObject
    {
        public float gameSystemTickTime = 0.02f;
        public float moveSystemMoveInterval = 0.05f;
        public float moveSystemAutoMoveInterval = 0.5f;
        public float horizontalMoveDelay = 0.5f;
        public float specialItemHitRate = 0.5f;
        
        public List<MissionSetting> missionSettings;
    }
}