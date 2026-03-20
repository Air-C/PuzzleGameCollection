using Controller;
using PGC.Enum;
using UnityEngine;

namespace PGC.ModuleMove
{
    public class MoveSystem
    {
        ShapeController shapeController = new ShapeController();
        GridController gridController = new GridController();
        
        private float moveInterval;
        private float autoMoveInterval;
        private float sinceLastMoveTime = 0.0f;
        private float sinceLastAutoMoveTime = 0.0f;

        public MoveSystem(GameContext ctx)
        {
            moveInterval = ctx.assetModule.sysSettings.moveSystemMoveInterval;
            autoMoveInterval = ctx.assetModule.sysSettings.moveSystemAutoMoveInterval;
            Debug.Log($"System moveInterval: {moveInterval}");
            Debug.Log($"System autoMoveInterval: {autoMoveInterval}");
        }
        
        public void Update(GameContext ctx)
        {
            ctx.inputModule.Update(Time.deltaTime);

        }

        public void Tick(GameContext ctx)
        {
            sinceLastMoveTime += ctx.assetModule.sysSettings.gameSystemTickTime;
            sinceLastAutoMoveTime += ctx.assetModule.sysSettings.gameSystemTickTime;
            if (sinceLastAutoMoveTime >= autoMoveInterval)
            {
                MoveDown(ctx,-1);
                sinceLastAutoMoveTime -= autoMoveInterval;
            }
            if (sinceLastMoveTime < moveInterval)
            {
                return;
            }
            if (ctx.inputModule.verticalMove.value != 0)
            {
                MoveDown(ctx);
            }
            
            if (ctx.inputModule.horizontalMove.value != 0 && gridController.IsCanHorizontalMove(ctx))
            {
                if (ctx.inputModule.holdTimer == 0 || ctx.inputModule.holdTimer >= ctx.inputModule.horizontalMoveDelay)
                {
                    shapeController.ShapeHorizontalMove(ctx.inputModule.horizontalMove.value,ctx);
                }
                Debug.Log($"holdTimer: {ctx.inputModule.holdTimer}；value: {ctx.inputModule.horizontalMove.value}");
                ctx.inputModule.holdTimer += moveInterval;
            }
            
            if (ctx.inputModule.rotate.value != 0 && gridController.IsCanRotate(ctx))
            {
                shapeController.ShapeRotate(ctx.inputModule.rotate.value,ctx);
            }
            ctx.inputModule.RestInput();
            sinceLastMoveTime -= moveInterval;
        }
        
        void MoveDown(GameContext ctx, int verticalMoveValue = 0)
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