using System;
using System.Collections.Generic;
using System.IO;
using PGC.Enum;
using UnityEngine;

namespace PGC.ModuleInventory.SO
{
    public class SaveDataSystem
    {
        private SaveData saveData = new SaveData();

        public SaveData SaveData
        {
            get => saveData;
        }

        public SaveDataSystem()
        {
            saveData.itemTypes = new List<ItemAbilityType>();
        }
        
        public bool SaveInventory(GameContext ctx)
        {
            try
            {
                foreach (var item in ctx.inventoryLocalData.GetInventoryItems(ctx))
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        saveData.itemTypes.Add(item.Value.Slot.AbilityType);
                    }
                }
                var json = JsonUtility.ToJson(saveData);
                Directory.CreateDirectory(GetPath());
                File.WriteAllText(GetPath(), json);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
            return true;
        }
        

        public void LoadData()
        {
            try
            {
                if (File.Exists(GetPath()))
                {
                    var json = File.ReadAllText(GetPath());
                    saveData = JsonUtility.FromJson<SaveData>(json);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        string GetPath(string key = "save.json")
        {
            return Path.Combine(Application.persistentDataPath, key);
        }
        
        public List<ItemEntity> items = new List<ItemEntity>();
        
    }
}