using System;
using System.Collections.Generic;
using System.Linq;
using PGC.Enum;
using PGC.ModelEvent.Data;
using PGC.ModuleClearLine.Model;
using UnityEngine;

namespace PGC.ModuleClearLine
{
    public class ClearLineSystem
    {
        private GameContext ctx;

        public ClearLineSystem(GameContext ctx)
        {
            this.ctx = ctx;
        }
        
        public void ClearWaitForClearModel()
        {
            while (ctx.clearModels.Count > 0)
            {
                OnClear(ctx.clearModels.Dequeue());
            }
        }

        public void OnClear(WaitForClearModel e)
        {
            List<Vector2Int> indexs = new List<Vector2Int>();
            switch (e.clearSquareType)
            {
                case ClearSquareType.Row:
                    foreach (var row in e.waitForClearRows)
                    {
                        for (int x = 0; x < ctx.grid.GridBorder.x; x++)
                        {
                            indexs.Add(new Vector2Int(x,row));
                        }
                    }
                    break;
                case ClearSquareType.Column:
                    foreach (var column in e.waitForClearColumns)
                    {
                        for (int y = 0; y < ctx.grid.GridBorder.y; y++)
                        {
                            indexs.Add(new Vector2Int(column,y));
                        }
                    }
                    break;
                case ClearSquareType.Circle:
                    CountCircleSquares(e,indexs);
                    break;
                default:
                    break;
            }
            ClearSquares(indexs);
        }

        void ClearSquares(List<Vector2Int> indexes)
        {
            Debug.Log($"OnClearInvoke:{indexes.ToArray()}");

            List<ItemAbilityType> itemTypes = new ();

            foreach (var index in indexes)
            {
                if (ctx.grid.Get(index.x, index.y) == null)
                {
                    SquareDown(index);
                    continue;
                }
                if (ctx.grid.Get(index.x,index.y).AbilityType != ItemAbilityType.None)
                {
                    //todo 销毁特殊方块获取道具
                    itemTypes.Add(ctx.grid.Get(index.x,index.y).AbilityType);
                }
                ctx.squaresWaitForDestroy.squares.Add(ctx.grid.Get(index.x,index.y));
                ctx.grid.ClearCell(index.x,index.y);
                SquareDown(index);
            }
            
            if (itemTypes.Count > 0)
            {
                AddItemEvent e = new AddItemEvent(itemTypes);
                ctx.eventBus.Publish(e);
            }
            ctx.eventBus.Publish<SquareClearedEvent>(new SquareClearedEvent(indexes));
        }

        void SquareDown(Vector2Int index)
        {
            if (ctx.grid.GridIndexIsNotNull(index.x, index.y))
            {
                return;
            }
            if (index.y >= ctx.grid.GridBorder.y)
            {
                return;
            }
            if (ctx.grid.GridIndexIsNotNull(index.x, index.y+1))
            {
                ctx.grid.Set(index.x,index.y,ctx.grid.Get(index.x,index.y+1));
                ctx.grid.Get(index.x,index.y).RestIndex(index.x,index.y);
                ctx.grid.Set(index.x,index.y+1,null);
            }
            SquareDown(new Vector2Int(index.x,index.y+1));
        }

        void CountCircleSquares(WaitForClearModel e,List<Vector2Int> indexs)
        {
            Vector2Int index = e.waitForClearCircle.index;
            int radius = e.waitForClearCircle.radius;
            int startX = index.x - radius > 0 ?  index.x - radius : 0;
            int endX = index.x + radius < ctx.grid.GridBorder.x ? index.x + radius : ctx.grid.GridBorder.x;
            int startY = index.y - radius > 0 ?  index.y - radius : 0;
            int endY = index.y + radius < ctx.grid.GridBorder.y ? 1 : ctx.grid.GridBorder.y;
            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    int disX = Math.Abs(x - index.x);
                    int disY = Math.Abs(y - index.y);
                    double dist = Math.Sqrt(disX * disX + disY * disY);
                    if (dist <= radius)
                    {
                        indexs.Add(new Vector2Int(x,y));
                    }
                }
            }
        }

    }
}