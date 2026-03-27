using System.Collections.Generic;
using PGC;
using PGC.System;
using UnityEngine;

namespace Controller
{
    public class MissionController
    {

        private int missionLevel = 1;
        private GameContext ctx;
        ShapeController shapeController;
        GridController gridController;

        Dictionary<int, MissionSetting> missionSettings = new Dictionary<int, MissionSetting>();


        public MissionController(GameContext ctx)
        {
            this.ctx = ctx;
            shapeController = new ShapeController(ctx);
            gridController = new GridController();
            foreach (var missionSetting in ctx.assetModule.sysSettings.missionSettings)
            {
                missionSettings.Add(missionSetting.missionID, missionSetting);
            }
        }
        
        void SetCurrentMission(int level)
        {
            if (missionSettings.TryGetValue(level, out MissionSetting missionSetting))
            {
                // todo 配置改用currentMissionSetting
                ctx.assetModule.currentMissionSetting = missionSetting;
            }
        }

        public void updateMission()
        {
            if (!ctx.gameSystemState.isRunning)
            {
                return;
            }
            
            shapeController.SpawnShapeRandom(ctx);
            
        }
        
        
    }
}