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

        
        public void SpawnShapeRandom(GameContext ctx)
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
            ctx.assetModule.SquareSoTryGetRandom(out SquareSo squareSo);
            Vector2Int initIndex = ctx.assetModule.gridSo.gridTopCenter;
            
            foreach (var offset in shapeSo.offsets)
            {
                Vector2Int squareIndex = initIndex + offset;
                if (ctx.grid.Get(squareIndex.x, squareIndex.y) != null)
                {
                    //GameOver!
                    GameOverEvent gameOverEvent = new GameOverEvent();
                    ctx.eventBus.Publish(gameOverEvent);
                    Debug.Log("Game Over!");
                    return;
                }
                
                SquareEntity square = new SquareEntity(squareIndex.x, squareIndex.y);
                square.SquareObj = UnityEngine.Object.Instantiate(squareSo.prefab, ctx.grid.GetWorldPositionByIndex(squareIndex), Quaternion.identity);
                if (squareSo.score != 0)
                {
                    square.Score = squareSo.score;
                }
                if (offset.x == 0 && offset.y == 0)
                {
                    ctx.currentSquareShape.AddSquaresFirst(square);
                }
                else
                {
                    ctx.currentSquareShape.AddSquare(square);
                }
            }
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

        public void ShapeHorizontalMove(int horizontalValue,GameContext ctx)
        {
            List<SquareEntity> squares = ctx.currentSquareShape.GetSquares();
            foreach (var square in squares)
            {
                Vector2Int newIndex = ConstantTool.GetNewIndexOfHorizontalMove(square.X, square.Y, horizontalValue);
                square.X = newIndex.x;
            }
            
        }

        public void ShapeDown(int verticalValue,GameContext ctx)
        {
            List<SquareEntity> squares = ctx.currentSquareShape.GetSquares();
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
        }
        
        
        
    }
}