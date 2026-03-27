using System;
using System.Collections.Generic;
using System.IO;
using PGC.Enum;
using PGC.ModelEvent.Data;
using PGC.ModuleInventory.SO;
using UnityEngine;

namespace PGC.ModuleInventory
{
    public class InventoryLocalData
    {
        
        private static InventoryLocalData instance;
        private GameContext ctx;
        
        private Dictionary<ItemAbilityType, InventorySlot> inventory = new ();
        
        public static InventoryLocalData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InventoryLocalData();
                }
                return instance;
            }
        }

        public void OnAddItemsToInventory(AddItemEvent e)
        {
            foreach (var item in e.items)
            {
                ctx.assetModule.itemTable.TryGetValue(item, out ItemEntity itemEntity);
                if (itemEntity != null)
                {
                    SaveInventoryItem(itemEntity);
                }
            }
            
        }

        public void SaveInventoryItem(ItemEntity item)
        {
            InventorySlot slot = new InventorySlot(item);
            inventory.Add(item.AbilityType, slot);
        }

        public Dictionary<ItemAbilityType, InventorySlot> GetInventoryItems(GameContext ctx)
        {
            List<ItemAbilityType> itemIds = ctx.saveData.itemTypes;
            if (inventory.Count != 0 || itemIds.Count == 0)
            {
                return inventory;
            }
            
            Dictionary<ItemAbilityType,ItemEntity> itemsTmp = ctx.assetModule.itemTable;
            foreach (var id in itemIds)
            {
                itemsTmp.TryGetValue(id, out ItemEntity item);
                if (item != null)
                {
                    inventory.TryGetValue(item.AbilityType, out InventorySlot slot);
                    if (slot != null)
                    {
                        slot.Count++;
                    }
                    else
                    {
                        slot = new InventorySlot(item);
                        inventory.Add(item.AbilityType, slot);
                    }
                }
            }
            
            return inventory;
        }


        
        
        
    }
}