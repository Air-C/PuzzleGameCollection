using System.Collections.Generic;
using Custom.Tool;
using Entities;
using PGC.Pool;
using UnityEngine;

namespace PGC.System
{
    public class RenderSystem
    {
        
        public static void RenderDestroySquare(GameContext ctx)
        {
            if (ctx.SquaresWaitForDestory.Count == 0)
            {
                return;
            }
            foreach (var square in ctx.SquaresWaitForDestory)
            {
                Object.Destroy(square.SquareObj);
            }
            ctx.SquaresWaitForDestory.Clear();

            foreach (var square in ctx.grid.Grid)
            {
                if (square != null)
                {
                    Vector3 pos = ctx.grid.GetWorldPositionByIndex(new Vector2Int(square.X,square.Y));
                    square.SquareObj.transform.position = pos;
                }
            }
        }
        
        
        public static void RenderShapePos(GameContext ctx)
        {
            List<SquareEntity> squares = ctx.squareShape.GetSquares();
            if (ctx.NewReacdhedSquare.Count > 0)
            {
                squares.AddRange(ctx.NewReacdhedSquare);
            }
            if (squares.Count == 0)
            {
                return;
            }

            foreach (var square in squares)
            {
                Vector3 pos = ctx.grid.GetWorldPositionByIndex(new Vector2Int(square.X,square.Y));
                square.SquareObj.transform.position = pos;
            }
        }
    }
}