using System;
using System.Collections;
using System.Collections.Generic;
using Entities.SO;
using PGC.Entities.Grid.SO;
using PGC.Enum;
using PGC.ModuleInventory;
using PGC.ModuleInventory.SO;
using PGC.Pool.SO;
using PGC.Settings;
using PGC.System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

namespace PGC.ModuleAsset
{
    public class AssetModule
    {
        readonly List<SquareSo> squareSoList = new ();
        readonly List<SquareShapeSo> squareShapeSoList = new ();
        public GridSo gridSo;
        public GameObject destroyParticleSystemPrefab;
        public Dictionary<ShapeTypeEnum, Sprite> shapePreviewPrefabDictionary = new ();
        public Dictionary<PopupEnum, GameObject> popupPrefabs = new ();
        public PoolSettings poolSettings;
        public SystemSettings sysSettings;
        public MissionSetting currentMissionSetting;
        public Dictionary<ItemAbilityType, ItemEntity> itemTable = new ();
        
        public IEnumerator LoadAllAssets()
        {
            yield return LoadAssets<SquareSo>("Square", (squareList) =>
            {
                squareSoList.AddRange(squareList);  
            });

            yield return LoadAssets<SquareShapeSo>("Shape", (shapeList) =>
            {
                squareShapeSoList.AddRange(shapeList);
            });
            
            yield return LoadAsset<GridSo>("GridSo", (gridSoAsset) =>
            {
                gridSo = gridSoAsset;
            });
            

            yield return LoadAsset<GameObject>("DestroyParticle", (particle) =>
            {
                destroyParticleSystemPrefab = particle;
            });
            
            yield return LoadAsset<PoolSettings>("ParticlePoolSettings", (settings) =>
            {
                poolSettings = settings;
            });
            
            yield return LoadAsset<SystemSettings>("PGCSettings", (settings) =>
            {
                sysSettings = settings;
                currentMissionSetting = new MissionSetting();
                currentMissionSetting.missionID = 1;
                currentMissionSetting.moveSystemMoveInterval = sysSettings.moveSystemMoveInterval;
                currentMissionSetting.moveSystemAutoMoveInterval = sysSettings.moveSystemAutoMoveInterval;
            });
            
            yield return LoadAsset<ItemTable>("ItemTable", (table) =>
            {
                foreach (var itemEntity in table.itemList)
                {
                    itemTable.Add(itemEntity.AbilityType, itemEntity);
                }
            });
            
            yield return LoadAssets<GameObject>("Popup", (popups) =>
            {
                foreach (var popup in popups)
                {
                    bool exist = global::System.Enum.TryParse<PopupEnum>(popup.name, out PopupEnum popupEnum);
                    if (exist)
                    {
                        popupPrefabs.Add(popupEnum, popup);
                    }
                    else
                    {
                        Debug.LogError($"{popup.name} is not define");
                    }
                }
            });

            yield return LoadAssets<Sprite>("ShapeImage", (shapeImagePrefabs) =>
            {
                foreach (var sprite in shapeImagePrefabs)
                {
                    bool exist = global::System.Enum.TryParse<ShapeTypeEnum>(sprite.name, out ShapeTypeEnum shapeTypeEnum);
                    if (exist)
                    {
                        shapePreviewPrefabDictionary.Add(shapeTypeEnum, sprite);
                    }
                    else
                    {
                        Debug.LogError($"{sprite.name} is not define");
                    }
                    
                }
            });
        }
        
        public IEnumerator LoadAssets<T>(string label, Action<IList<T>> callback)
        {
            Debug.Log($"Loading assets: {label}");
            var handle =  Addressables.LoadAssetsAsync<T>(label, null);
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"LoadAssets<{label}> failed: {handle.Status}");
                yield break;
            }
            callback?.Invoke(handle.Result);
        }
        
        public IEnumerator LoadAsset<T>(string key, Action<T> callback)
        {
            Debug.Log($"Loading asset: {key}");
            var handle =  Addressables.LoadAssetAsync<T>(key);
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"LoadAsset<{key}> failed: {handle.Status}");
                yield break;
            }
            callback?.Invoke(handle.Result);
        }

        public bool ShapeSoTryGetRandom(out SquareShapeSo squareShapeSo)
        {
            if (squareShapeSoList == null || squareShapeSoList.Count == 0)
            {
                squareShapeSo = null;
                return false;
            }
            int index = Random.Range(0, squareShapeSoList.Count);
            squareShapeSo = squareShapeSoList[index];
            return true;
        }

        public bool SquareSoTryGetRandom(out SquareSo squareSo)
        {
            if (squareSoList == null || squareSoList.Count == 0)
            {
                squareSo = null;
                return false;
            }
            int index = Random.Range(0, squareSoList.Count);
            squareSo = squareSoList[index];
            return true;
        }

        public bool GetPopup(PopupEnum type, out GameObject popup)
        {
            bool exist = popupPrefabs.TryGetValue(type, out popup);
            if (!exist)
            {
                Debug.LogError($"{type.ToString()} is not Loaded");
                return false;
            }
            return true;
        }

        public bool GetShapePreviewSprite(ShapeTypeEnum type, out Sprite shapePreviewSprite)
        {
            bool exist = shapePreviewPrefabDictionary.TryGetValue(type, out shapePreviewSprite);
            if (!exist)
            {
                Debug.LogError($"{type.ToString()} is not Loaded");
                return false;
            }
            return true;
        }
        
    }
}