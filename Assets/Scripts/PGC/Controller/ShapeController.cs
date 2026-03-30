using System.Collections.Generic;
using Custom.Tool;
using Entities;
using Entities.SO;
using PGC;
using PGC.ModelEvent.Data;
using UnityEngine;

namespace Controller
{
    public class ShapeController
    {
        private GameContext ctx;

        public ShapeController(GameContext ctx)
        {
            this.ctx = ctx;
        }

        
        public void SpawnShapeRandom()
        {
            if (ctx.currentSquareShape.GetSquares().Count > 0)
            {
                return;
            }

            if (ctx.previewShape == null)
            {
                GetPreviewShapeSo();
            }
            SquareShapeSo shapeSo = ctx.previewShape;
            Vector2Int initIndex = ctx.assetModule.gridSo.gridTopCenter;
            List<Vector2Int> squareIndexes = new List<Vector2Int>();
            foreach (var offset in shapeSo.offsets)
            {
                Vector2Int squareIndex = initIndex + offset;
                squareIndexes.Add(squareIndex);
                if (ctx.grid.Get(squareIndex.x, squareIndex.y) != null)
                {
                    //GameOver!
                    GameOverEvent gameOverEvent = new GameOverEvent();
                    ctx.eventBus.Publish(gameOverEvent);
                    Debug.Log("Game Over!");
                    return;
                }
            }
            List<SquareEntity> squareEntityList = ctx.squarePool.GetSquareByIndexes(squareIndexes);
            foreach (var squareEntity in squareEntityList)
            {
                if (squareEntity.X-initIndex.x == 0 && squareEntity.Y-initIndex.y == 0)
                {
                    ctx.currentSquareShape.AddSquaresFirst(squareEntity);
                }
                else
                {
                    ctx.currentSquareShape.AddSquare(squareEntity);
                }
            }
            // 设置形状类型
            ctx.currentSquareShape.ShapeType = shapeSo.type;
            ResetShapePreview();
        }

        public void ResetShapePreview()
        {
            ctx.previewShape = null;
            GetPreviewShapeSo();
            PreviewShapeChangeEvent evt = new PreviewShapeChangeEvent();
            if (ctx.previewShape == null)
            {
                return;
            }
            evt.shapeType = ctx.previewShape.type;
            ctx.assetModule.GetShapePreviewSprite(evt.shapeType,out evt.shapePreviewSprite);
            ctx.eventBus.Publish(evt);
        }     
        
        public void GetPreviewShapeSo()
        {
            ctx.assetModule.ShapeSoTryGetRandom(out ctx.previewShape);
        }

        public void ShapeHorizontalMove(int horizontalValue)
        {
            List<SquareEntity> squares = ctx.currentSquareShape.GetSquares();
            foreach (var square in squares)
            {
                Vector2Int newIndex = ConstantTool.GetNewIndexOfHorizontalMove(square.X, square.Y, horizontalValue);
                square.X = newIndex.x;
            }
            
        }

        public void ShapeDown(int verticalValue)
        {
            List<SquareEntity> squares = ctx.currentSquareShape.GetSquares();
            foreach (var square in squares)
            {
                Vector2Int newIndex = ConstantTool.GetNewIndexOfVerticalMove(square.X, square.Y,verticalValue);
                square.Y = newIndex.y;
            }
        }

        public void ShapeRotate(int rotateValue)
        {
            if (rotateValue == 0)
            {
                return;
            }
            List<SquareEntity> squares = ctx.currentSquareShape.GetSquares();
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
            
            // 应用wallkick位移
            if (ctx.currentSquareShape.waitForExcWallKick != null && ctx.currentSquareShape.waitForExcWallKick.Length > 0)
            {
                Vector2Int offset = ctx.currentSquareShape.waitForExcWallKick[0];
                foreach (var square in squares)
                {
                    square.X += offset.x;
                    square.Y += offset.y;
                }
                ctx.currentSquareShape.ClearWallKick();
            }
            
            ctx.currentSquareShape.RotateStatusMoveNext();
        }
        
        
        
    }
}