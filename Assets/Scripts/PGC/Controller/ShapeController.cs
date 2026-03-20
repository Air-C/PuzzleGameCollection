using System.Collections.Generic;
using Custom.Tool;
using Entities;
using Entities.SO;
using PGC;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class ShapeController
    {
        
        public void SpawnShapeRandom(GameContext ctx)
        {
            if (ctx.squareShape.GetSquares().Count > 0)
            {
                return;
            }
            
            ctx.assetModule.ShapeSoTryGetRandom(out SquareShapeSo shapeSo);
            ctx.assetModule.SquareSoTryGetRandom(out SquareSo squareSo);
            Vector2Int initIndex = ctx.assetModule.gridSo.gridTopCenter;
        
            foreach (var offset in shapeSo.offsets)
            {

                Vector2Int squareIndex = initIndex + offset;
                SquareEntity square = new SquareEntity(squareIndex.x, squareIndex.y);
                square.SquareObj = UnityEngine.Object.Instantiate(squareSo.prefab, ctx.grid.GetWorldPositionByIndex(squareIndex), Quaternion.identity);
                if (offset.x == 0 && offset.y == 0)
                {
                    ctx.squareShape.AddSquaresFirst(square);
                }
                else
                {
                    ctx.squareShape.AddSquare(square);
                }
            }
            
            
        }

        public void ShapeHorizontalMove(int horizontalValue,GameContext ctx)
        {
            List<SquareEntity> squares = ctx.squareShape.GetSquares();
            foreach (var square in squares)
            {
                Vector2Int newIndex = ConstantTool.GetNewIndexOfHorizontalMove(square.X, square.Y, horizontalValue);
                square.X = newIndex.x;
            }
            
        }

        public void ShapeDown(int verticalValue,GameContext ctx)
        {
            List<SquareEntity> squares = ctx.squareShape.GetSquares();
            foreach (var square in squares)
            {
                Vector2Int newIndex = ConstantTool.GetNewIndexOfVerticalMove(square.X, square.Y,verticalValue);
                square.Y = newIndex.y;
                // Vector3 worldPos = ctx.grid.GetWorldPositionByIndex(new Vector2Int(square.X, square.Y));
                // square.SquareObj.transform.position = worldPos;
            }
        }

        public void ShapeRotate(int rotateValue,GameContext ctx)
        {
            if (rotateValue == 0)
            {
                return;
            }
            List<SquareEntity> squares = ctx.squareShape.GetSquares();
            SquareEntity pivotSquare = null;
            foreach (var square in squares)
            {
                if (pivotSquare == null)
                {
                    pivotSquare = square;
                    continue;
                }
                Vector2Int newIndex = ConstantTool.GetNewIndexOfRotate(square.X, square.Y, pivotSquare);
                square.X = newIndex.x ;
                square.Y = newIndex.y ;
            }
        }
        
        
        
    }
}