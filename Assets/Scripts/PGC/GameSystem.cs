using System;
using System.Collections;
using Controller;
using PGC.Enum;
using PGC.ModuleMove;
using PGC.System;
using UnityEngine;
namespace PGC
{

    using UnityEngine;
    
    public class GameSystem
    {
        ShapeController shapeController = new ShapeController();

        MoveSystem moveSystem;
        private float restTime;
        private float tickTime;

        
        
        public GameSystem(GameContext ctx)
        {
            moveSystem = new MoveSystem(ctx);
            tickTime = ctx.assetModule.sysSettings.gameSystemTickTime;
            Debug.Log($"System tick time: {tickTime}");
        }

        public void InitGame(GameContext ctx)
        {
            if (ctx.squareShape.GetSquares().Count == 0)
            {
                shapeController.SpawnShapeRandom(ctx);
            }
        }
        
        public void Tick(GameContext ctx)
        {
            float dt = Time.deltaTime;
            restTime += dt;
            while (restTime >= tickTime)
            {
                moveSystem.Tick(ctx);
                restTime -= tickTime;
            }
            
        }

        public void UpdateGame(GameContext ctx)
        {
            if (ctx.testFlag)
            {
                Debug.Log("testFlag");
                
            }
            
            moveSystem.Update(ctx);
            RenderSystem.RenderDestroySquare(ctx);
            shapeController.SpawnShapeRandom(ctx);
            RenderSystem.RenderShapePos(ctx);

        }





        
    }
}