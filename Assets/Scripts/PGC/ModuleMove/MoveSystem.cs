using Controller;
using PGC.Enum;
using UnityEngine;

namespace PGC.ModuleMove
{
    public class MoveSystem
    {
        ShapeController shapeController;
        GridController gridController;
        
        private float sinceLastMoveTime = 0.0f;
        private float sinceLastAutoMoveTime = 0.0f;
        private GameContext ctx;

        public MoveSystem(GameContext ctx)
        {
            this.ctx = ctx;
            shapeController = new ShapeController(ctx);
            gridController = new GridController(ctx);
        }
        
        public void Update()
        {
            ctx.inputModule.Update(Time.deltaTime);
        }

        public void Tick()
        {
            sinceLastMoveTime += ctx.assetModule.sysSettings.gameSystemTickTime;
            sinceLastAutoMoveTime += ctx.assetModule.sysSettings.gameSystemTickTime;
            if (sinceLastAutoMoveTime >= ctx.assetModule.currentMissionSetting.moveSystemAutoMoveInterval)
            {
                MoveDown(-1);
                sinceLastAutoMoveTime -= ctx.assetModule.currentMissionSetting.moveSystemAutoMoveInterval;
            }
            if (sinceLastMoveTime < ctx.assetModule.currentMissionSetting.moveSystemMoveInterval)
            {
                return;
            }
            if (ctx.inputModule.verticalMove.value != 0)
            {
                MoveDown();
            }
            
            if (ctx.inputModule.horizontalMove.value != 0 && gridController.IsCanHorizontalMove())
            {
                if (ctx.inputModule.holdTimer == 0 || ctx.inputModule.holdTimer >= ctx.assetModule.currentMissionSetting.horizontalMoveDelay)
                {
                    shapeController.ShapeHorizontalMove(ctx.inputModule.horizontalMove.value);
                }
                ctx.inputModule.holdTimer += ctx.assetModule.currentMissionSetting.moveSystemMoveInterval;
            }
            
            if (ctx.inputModule.rotate.value != 0 && gridController.IsCanRotate())
            {
                shapeController.ShapeRotate(ctx.inputModule.rotate.value);
            }

            if (ctx.inputModule.isPressedChange)
            {
                shapeController.ResetShapePreview();
            }
            ctx.inputModule.RestInput();
            sinceLastMoveTime -= ctx.assetModule.currentMissionSetting.moveSystemMoveInterval;
        }
        
        void MoveDown(int verticalMoveValue = 0)
        {
            if (verticalMoveValue == 0)
            {
                verticalMoveValue = ctx.inputModule.verticalMove.value;
            }
            
            if (gridController.IsCanDown(verticalMoveValue))
            {
                shapeController.ShapeDown(verticalMoveValue);
            }
            else
            {
                gridController.LockSquares();
                gridController.CheckIsFullAndClear();
            }
        }
    }
}