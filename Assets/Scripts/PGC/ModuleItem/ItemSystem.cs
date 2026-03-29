using System;
using System.Collections.Generic;
using Controller;
using Entities;
using Entities.SO;
using PGC.Enum;
using PGC.ModelEvent.Data;
using PGC.ModuleClearLine.Model;
using PGC.ModuleInventory;
using PGC.ModuleItem.Model;
using UnityEngine;

namespace PGC.ModuleItem
{
    public class ItemSystem
    {
        private GameContext ctx;
        private Dictionary<ItemAbilityType, Action> positiveActions;
        private Dictionary<ItemAbilityType, Action> negativeActions;
        private GridController gridController;
        private ShapeController shapeController;
        public ItemSystem(GameContext ctx)
        {
            this.ctx = ctx;
            gridController = new GridController(ctx);
            shapeController = new ShapeController(ctx);
            positiveActions = new Dictionary<ItemAbilityType, Action>()
            {
                {ItemAbilityType.Boom2, Boom},
                {ItemAbilityType.Boom3, Boom},
                {ItemAbilityType.ClearColumn , ClearColumn},
                {ItemAbilityType.ClearRow, ClearRow},
                
            };
            negativeActions = new Dictionary<ItemAbilityType, Action>()
            {
                {ItemAbilityType.AddRow1, AddRow1},
            };
        }

        public void OnGenerateItem(AddItemEvent e)
        {
            foreach (var itemAbilityType in e.items)
            {
                ctx.assetModule.itemTable.TryGetValue(itemAbilityType, out ItemEntity item);
                if (item == null)
                {
                    Debug.LogWarning($"itemTable not found: {itemAbilityType}");
                    continue;
                }
                ItemAbilityType barType = SetItemToBar(item);
                if (barType != ItemAbilityType.None)
                {
                    // Object.Instantiate(item.Prefab, bar.transform);
                    ctx.eventBus.Publish(new ItemBarChangeEvent()
                    {
                        type = barType,
                        itemPrefab = item.Prefab
                    });
                }
            }
        }

        ItemAbilityType SetItemToBar(ItemEntity item)
        {
            ItemBarModel noneBar = null;
            foreach (var itemBar in ctx.itemsBar)
            {
                if (itemBar.type == ItemAbilityType.None && noneBar == null)
                {
                    noneBar = itemBar;
                }
                if (itemBar.type == item.AbilityType)
                {
                    itemBar.count++;
                    return itemBar.type;
                }
            }

            if (noneBar != null)
            {
                noneBar.type = item.AbilityType;
                noneBar.count = 1;
            }
            else
            {
                Debug.LogWarning("ItemBar has consumed");
            }
            return noneBar?.type ?? ItemAbilityType.None;
        }
        
        public void OnPositiveItemEffect(ItemEffectEvent e)
        {
            if (positiveActions.TryGetValue(e.type, out Action action))
            {
                action.Invoke();
            }
        }

        public void OnNegativeItemEffect(ItemEffectEvent e)
        {
            if (negativeActions.TryGetValue(e.type, out Action action))
            {
                action.Invoke();
            }
            
        }

        void AddRow1()
        {
            // 棋盘数据向上移动
            for (int x = 0; x < ctx.grid.GridBorder.x; x++)
            {
                for (int y = ctx.grid.GridBorder.y; y > 0; y--)
                {
                    if (!ctx.grid.GridIndexIsNotNull(x, y))
                    {
                        continue;
                    }
                    int newRow = y + 1;
                    ctx.grid.Set(x,newRow, ctx.grid.Get(x,y));
                    ctx.grid.Get(x,newRow)?.RestIndex(x,newRow);
                    ctx.grid.Set(x, y,null);
                }
            }
            // 最低层一行增加新方块
            List<Vector2Int> indexes = new List<Vector2Int>();
            for (int x = 0; x < ctx.grid.GridBorder.x; x++)
            {
                indexes.Add(new Vector2Int(x, 0));
            }
            List<SquareEntity> squareEntityList = ctx.squarePool.GetSquareByIndexes(indexes);
            foreach (var squareEntity in squareEntityList)
            {
                ctx.grid.Set(squareEntity.X, squareEntity.Y, squareEntity);
            }
        }

        void ClearRow()
        {
            WaitForClearModel waitForClearModel = new WaitForClearModel();
            waitForClearModel.clearSquareType = ClearSquareType.Row;
            waitForClearModel.waitForClearRows = new HashSet<int>(){0};
            ctx.clearModels.Enqueue(waitForClearModel);
        }

        void ClearColumn()
        {
            WaitForClearModel waitForClearModel = new WaitForClearModel();
            waitForClearModel.clearSquareType = ClearSquareType.Column;
            waitForClearModel.waitForClearColumns = new HashSet<int>(){0};
            ctx.clearModels.Enqueue(waitForClearModel);
        }

        void Boom()
        {
            WaitForClearModel waitForClearModel = new WaitForClearModel();
            waitForClearModel.clearSquareType = ClearSquareType.Circle;
            waitForClearModel.waitForClearCircle = (new Vector2Int(0,0), 3);
            ctx.clearModels.Enqueue(waitForClearModel);
        }
    }
}