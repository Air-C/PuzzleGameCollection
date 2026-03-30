using System.Collections.Generic;
using System.Linq;
using Custom.Tool;
using Entities;
using Entities.SO;
using PGC;
using PGC.Enum;
using PGC.ModelEvent.Data;
using PGC.ModuleClearLine.Model;
using UnityEngine;

namespace Controller
{
    public class GridController
    {
        private GameContext ctx;
        public GridController(GameContext ctx)
        {
            this.ctx = ctx;
        }

        public bool IsCanHorizontalMove()
        {
            List<SquareEntity> squares = ctx.currentSquareShape.GetSquares();
            if (squares.Count == 0)
            {
                return false;
            }
            int horizontalValue = ctx.inputModule.horizontalMove.value;
            foreach (var square in ctx.currentSquareShape.GetSquares())
            {
                Vector2Int newIndex = ConstantTool.GetNewIndexOfHorizontalMove(square.X, square.Y, horizontalValue);
                if (newIndex.x < 0 || newIndex.x >= ctx.grid.GridBorder.x || ctx.grid.GridIndexIsNotNull(newIndex.x,newIndex.y))
                {
                    return false;
                }
            }
            return true;
        }
        
        public bool IsCanDown( int verticalValue = 0)
        {
            List<SquareEntity> squares = ctx.currentSquareShape.GetSquares();

            if (verticalValue == 0)
            {
                verticalValue = ctx.inputModule.verticalMove.value;
            }
            if (squares.Count == 0 || verticalValue == 0)
            {
                return false;
            }
            
            foreach (var square in ctx.currentSquareShape.GetSquares())
            {
                Vector2Int newIndex = ConstantTool.GetNewIndexOfVerticalMove(square.X, square.Y, verticalValue);
                if (newIndex.y < 0 || ctx.grid.GridIndexIsNotNull(newIndex.x,newIndex.y))
                {
                    ctx.currentShapeIsReachedBottom = true;
                    return false;
                }
            }
            return true;
        }
        
        public bool IsCanRotate()
        {
            List<SquareEntity> squares = ctx.currentSquareShape.GetSquares();
            if (squares.Count == 0)
            {
                return false;
            }
            SquareEntity pivotSquare = null;
            foreach (var square in ctx.currentSquareShape.GetSquares())
            {
                if (pivotSquare == null)
                {
                    pivotSquare = square;
                    continue;
                }
                Vector2Int newIndex = ConstantTool.GetNewIndexOfRotate(square.X, square.Y, pivotSquare);
                
                if (newIndex.x < 0 || newIndex.x >= ctx.grid.GridBorder.x )
                {
                    return TryWallKick();
                }
                if (newIndex.y < 0 || ctx.grid.GridIndexIsNotNull(newIndex.x,newIndex.y ))
                {
                    ctx.currentShapeIsReachedBottom = true;
                    return TryWallKick();
                }
            }
            return true;
        }

        private bool TryWallKick()
        {
            // 计算旋转前后的状态
            ShapeRotateStatusEnum fromStatus = ctx.currentSquareShape.RotateStatus;
            ShapeRotateStatusEnum toStatus = ConstantTool.GetEnumNext(fromStatus);
            
            // 获取当前形状类型
            ShapeTypeEnum shapeType = ctx.currentSquareShape.ShapeType;
            
            // 获取wallkick规则
            Vector2Int[] wallKickRules = ctx.currentSquareShape.GetWallKickRules(fromStatus, toStatus, shapeType);
            if (wallKickRules == null)
            {
                return false;
            }
            
            // 尝试每个wallkick规则
            foreach (var offset in wallKickRules)
            {
                if (IsCanMoveWithOffset(offset))
                {
                    // 记录成功的wallkick规则
                    ctx.currentSquareShape.SetWallKick(new Vector2Int[] { offset });
                    return true;
                }
            }
            
            return false;
        }
        
                
        private bool IsCanMoveWithOffset(Vector2Int offset)
        {
            List<SquareEntity> squares = ctx.currentSquareShape.GetSquares();
            SquareEntity pivotSquare = squares[0];
            
            foreach (var square in squares)
            {
                Vector2Int newIndex = ConstantTool.GetNewIndexOfRotate(square.X, square.Y, pivotSquare);
                newIndex += offset;
                
                if (newIndex.x < 0 || newIndex.x >= ctx.grid.GridBorder.x)
                {
                    return false;
                }
                if (newIndex.y < 0 || ctx.grid.GridIndexIsNotNull(newIndex.x, newIndex.y))
                {
                    return false;
                }
            }
            
            return true;
        }

        public void LockSquares()
        {
            if (!ctx.currentShapeIsReachedBottom)
            {
                return;
            }
            
            foreach (var squareEntity in ctx.currentSquareShape.GetSquares())
            {
                ctx.grid.Set(squareEntity.X, squareEntity.Y, squareEntity);
                ctx.newReachedSquare.Add(squareEntity);
            }
            ctx.currentSquareShape.ClearSquares();
            ctx.currentShapeIsReachedBottom = false;
        }

        public void CheckIsFullAndClear()
        {
            if (ctx.newReachedSquare.Count == 0)
            {
                return;
            }
            
            HashSet<int> clearRows = new HashSet<int>();
            foreach (var squareEntity in ctx.newReachedSquare)
            {
                bool isFUll = true;
                for (int i = 0; i < ctx.grid.GridBorder.x; i++)
                {
                    if (!ctx.grid.GridIndexIsNotNull(i,squareEntity.Y))
                    {
                        isFUll = false;
                        break;
                    }
                }
                if (isFUll)
                {
                    clearRows.Add(squareEntity.Y);
                }
            }

            if (clearRows.Count > 0)
            {
                WaitForClearModel waitForClearModel = new WaitForClearModel();
                waitForClearModel.clearSquareType = ClearSquareType.Row;
                waitForClearModel.waitForClearRows = clearRows;
                ctx.clearModels.Enqueue(waitForClearModel);
            }
            ctx.newReachedSquare.Clear();
        }
        
        
    }
}