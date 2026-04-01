using System.Collections.Generic;
using PGC;
using PGC.ModelEvent.Data;
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
        List<int> missionIds = new List<int>();
        Dictionary<int, MissionSetting> missionSettings = new Dictionary<int, MissionSetting>();


        public MissionController(GameContext ctx)
        {
            this.ctx = ctx;
            shapeController = new ShapeController(ctx);
            gridController = new GridController(ctx);
            foreach (var missionSetting in ctx.assetModule.sysSettings.missionSettings)
            {
                missionSettings.Add(missionSetting.missionID, missionSetting);
                missionIds.Add(missionSetting.missionID);
            }
        }
        
        public void OnSetMissionLevel(MissionSetEvent e)
        {
            int index = e.code < missionIds.Count ? e.code : missionIds.Count - 1;
            int level = missionIds[index];
            SetCurrentMission(level);
        }
        
        public void SetCurrentMission(int level)
        {
            missionLevel = level;
            if (missionSettings.TryGetValue(level, out MissionSetting missionSetting))
            {
                // todo 配置改用currentMissionSetting
                ctx.assetModule.currentMissionSetting = missionSetting;
            }
        }

        public void UpdateMission()
        {
            if (!ctx.gameSystemState.isRunning)
            {
                return;
            }
            shapeController.SpawnShapeRandom();
        }
        
        
    }
}