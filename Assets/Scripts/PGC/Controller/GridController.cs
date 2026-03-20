using System.Collections.Generic;
using System.Linq;
using Custom.Tool;
using Entities;
using PGC;
using PGC.ModelEvent.Data;
using UnityEngine;

namespace Controller
{
    public class GridController
    {

        public bool IsCanHorizontalMove(GameContext ctx)
        {
            List<SquareEntity> squares = ctx.squareShape.GetSquares();
            if (squares.Count == 0)
            {
                return false;
            }
            int horizontalValue = ctx.inputModule.horizontalMove.value;
            foreach (var square in ctx.squareShape.GetSquares())
            {
                Vector2Int newIndex = ConstantTool.GetNewIndexOfHorizontalMove(square.X, square.Y, horizontalValue);
                if (newIndex.x < 0 || newIndex.x >= ctx.grid.GridBorder.x || ctx.grid.GridIndexIsNotNull(newIndex.x,newIndex.y))
                {
                    return false;
                }
            }
            return true;
        }
        
        public bool IsCanDown(GameContext ctx, int verticalValue = 0)
        {
            List<SquareEntity> squares = ctx.squareShape.GetSquares();

            if (verticalValue == 0)
            {
                verticalValue = ctx.inputModule.verticalMove.value;
            }
            if (squares.Count == 0 || verticalValue == 0)
            {
                return false;
            }
            
            foreach (var square in ctx.squareShape.GetSquares())
            {
                Vector2Int newIndex = ConstantTool.GetNewIndexOfVerticalMove(square.X, square.Y, verticalValue);
                if (newIndex.y < 0 || ctx.grid.GridIndexIsNotNull(newIndex.x,newIndex.y))
                {
                    ctx.CurrentShapeIsReachedBottom = true;
                    return false;
                }
            }
            return true;
        }
        
        public bool IsCanRotate(GameContext ctx)
        {
            List<SquareEntity> squares = ctx.squareShape.GetSquares();
            if (squares.Count == 0)
            {
                return false;
            }
            SquareEntity pivotSquare = null;
            foreach (var square in ctx.squareShape.GetSquares())
            {
                if (pivotSquare == null)
                {
                    pivotSquare = square;
                    continue;
                }
                Vector2Int newIndex = ConstantTool.GetNewIndexOfRotate(square.X, square.Y, pivotSquare);
                
                if (newIndex.x < 0 || newIndex.x >= ctx.grid.GridBorder.x )
                {
                    return false;
                }
                if (newIndex.y < 0 || ctx.grid.GridIndexIsNotNull(newIndex.x,newIndex.y ))
                {
                    ctx.CurrentShapeIsReachedBottom = true;
                    return false;
                }
            }
            return true;
        }

        public void LockSquares(GameContext ctx)
        {
            Debug.Log($"CurrentShapeIsReachedBottom:{ctx.CurrentShapeIsReachedBottom}");
            if (!ctx.CurrentShapeIsReachedBottom)
            {
                return;
            }
            
            foreach (var squareEntity in ctx.squareShape.GetSquares())
            {
                ctx.grid.Grid[squareEntity.X, squareEntity.Y] = squareEntity;
                ctx.grid.Set(squareEntity.X, squareEntity.Y, squareEntity);
                ctx.NewReacdhedSquare.Add(squareEntity);
            }
            ctx.squareShape.ClearSquares();
            ctx.CurrentShapeIsReachedBottom = false;
        }

        public void CheckIsFullAndClear(GameContext ctx)
        {
            if (ctx.NewReacdhedSquare.Count == 0)
            {
                return;
            }
            
            HashSet<int> clearRows = new HashSet<int>();
            foreach (var squareEntity in ctx.NewReacdhedSquare)
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
                ClearRow(clearRows,ctx);
            }
            List<int> lines = new List<int>(clearRows);
            LineClearedEvent e = new LineClearedEvent(lines);
            ctx.eventBus.Publish(e);
            ctx.NewReacdhedSquare.Clear();
        }

        void ClearRow(HashSet<int> rows, GameContext ctx)
        {
            int minRow = rows.Min();
            foreach (var row in rows)
            {
                for (int x = 0; x < ctx.grid.GridBorder.x; x++)
                {
                    //todo 对象销毁，特性等
                    //待销毁方块
                    ctx.SquaresWaitForDestory.Add(ctx.grid.Get(x,row));
                    ctx.grid.ClearCell(x,row);
                }
            }

            // 棋盘数据下移动
            for (int x = 0; x < ctx.grid.GridBorder.x; x++)
            {
                for (int y = minRow; y < ctx.grid.GridBorder.y; y++)
                {
                    int newRow = y + rows.Count;
                    if (newRow < ctx.grid.GridBorder.y)
                    {
                        ctx.grid.Set(x,y, ctx.grid.Get(x,y+rows.Count));
                        ctx.grid.Get(x,y)?.RestIndex(x,y);
                    }
                    else
                    {
                        ctx.grid.Set(x,y, null);
                    }
                }
            }
        }
        
    }
}