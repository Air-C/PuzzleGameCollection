using Controller;
using PGC.Enum;
using UnityEngine;

namespace PGC.ModuleMove
{
    public class MoveSystem
    {
        ShapeController shapeController;
        GridController gridController = new GridController();
        
        private float sinceLastMoveTime = 0.0f;
        private float sinceLastAutoMoveTime = 0.0f;
        private GameContext ctx;

        public MoveSystem(GameContext ctx)
        {
            this.ctx = ctx;
            shapeController = new ShapeController(ctx);
            Debug.Log($"System moveInterval: {ctx.assetModule.sysSettings.moveSystemMoveInterval}");
            Debug.Log($"System autoMoveInterval: {ctx.assetModule.sysSettings.moveSystemAutoMoveInterval}");
        }
        
        public void Update()
        {
            ctx.inputModule.Update(Time.deltaTime);
        }

        public void Tick()
        {
            sinceLastMoveTime += ctx.assetModule.sysSettings.gameSystemTickTime;
            sinceLastAutoMoveTime += ctx.assetModule.sysSettings.gameSystemTickTime;
            if (sinceLastAutoMoveTime >= ctx.assetModule.sysSettings.moveSystemAutoMoveInterval)
            {
                MoveDown(-1);
                sinceLastAutoMoveTime -= ctx.assetModule.sysSettings.moveSystemAutoMoveInterval;
            }
            if (sinceLastMoveTime < ctx.assetModule.sysSettings.moveSystemMoveInterval)
            {
                return;
            }
            if (ctx.inputModule.verticalMove.value != 0)
            {
                MoveDown();
            }
            
            if (ctx.inputModule.horizontalMove.value != 0 && gridController.IsCanHorizontalMove(ctx))
            {
                if (ctx.inputModule.holdTimer == 0 || ctx.inputModule.holdTimer >= ctx.assetModule.sysSettings.horizontalMoveDelay)
                {
                    shapeController.ShapeHorizontalMove(ctx.inputModule.horizontalMove.value,ctx);
                }
                ctx.inputModule.holdTimer += ctx.assetModule.sysSettings.moveSystemMoveInterval;
            }
            
            if (ctx.inputModule.rotate.value != 0 && gridController.IsCanRotate(ctx))
            {
                shapeController.ShapeRotate(ctx.inputModule.rotate.value,ctx);
            }
            ctx.inputModule.RestInput();
            sinceLastMoveTime -= ctx.assetModule.sysSettings.moveSystemMoveInterval;
        }
        
        void MoveDown(int verticalMoveValue = 0)
        {
            if (verticalMoveValue == 0)
            {
                verticalMoveValue = ctx.inputModule.verticalMove.value;
            }
            
            if (gridController.IsCanDown(ctx, verticalMoveValue))
            {
                shapeController.ShapeDown(verticalMoveValue,ctx);
            }
            else
            {
                gridController.LockSquares(ctx);
                gridController.CheckIsFullAndClear(ctx);
            }
        }
    }
}