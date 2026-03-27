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

        public void ExecuteClear()
        {
            if (ctx.waitForClearModelQueue.Count == 0)
            {
                return;
            }

            WaitForClearModel waitForClearModel;
            while (ctx.waitForClearModelQueue.Count > 0)
            {
                waitForClearModel = ctx.waitForClearModelQueue.Dequeue();
                if (waitForClearModel.clearSquareType == ClearSquareType.Row)
                {
                    ClearRow(waitForClearModel.waitForClearRows);
                }

                if (waitForClearModel.clearSquareType == ClearSquareType.Column)
                {
                    ClearColumn(waitForClearModel.waitForClearColumns);
                }

                if (waitForClearModel.clearSquareType == ClearSquareType.Circle)
                {
                    ClearCircle(waitForClearModel.waitForClearCircle.index, waitForClearModel.waitForClearCircle.radius);
                }
                
            }
        }
        
        void ClearRow(HashSet<int> rows)
        {
            
            int minRow = rows.Min();
            List<ItemAbilityType> itemTypes = new ();
            foreach (var row in rows)
            {
                for (int x = 0; x < ctx.grid.GridBorder.x; x++)
                {
                    //todo 对象销毁，特性等
                    //待销毁方块
                    if (ctx.grid.Get(x,row).AbilityType != ItemAbilityType.None)
                    {
                        //todo 销毁特殊方块获取道具
                        itemTypes.Add(ctx.grid.Get(x,row).AbilityType);
                    }

                    ctx.SquaresWaitForDestory.squares.Add(ctx.grid.Get(x,row));
                    ctx.grid.ClearCell(x,row);
                }
            }
            if (itemTypes.Count > 0)
            {
                AddItemEvent e = new AddItemEvent(itemTypes);
                ctx.eventBus.Publish(e);
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
            
            rows.Clear();
        }

        void ClearColumn(HashSet<int> columns)
        {
            
        }

        void ClearCircle(Vector2Int pivot, int radius)
        {
            
        }
    }
}