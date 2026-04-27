using System;
using System.Collections;
using System.Collections.Generic;
using Custom.Tool;
using Entities.SO;
using PGC.Entities.Grid.SO;
using PGC.Enum;
using PGC.ModuleAsset.Model;
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
        private GameContext ctx;
        readonly List<SquareSo> squareSoList = new ();
        readonly List<SquareShapeSo> squareShapeSoList = new ();
        readonly Queue<SquareShapeSo> shapeSoBagQueue = new ();
        public GridSo gridSo;
        public GameObject destroyParticleSystemPrefab;
        public Dictionary<ShapeTypeEnum, Sprite> shapePreviewPrefabDictionary = new ();
        public Dictionary<PopupEnum, GameObject> popupPrefabs = new ();
        public ParticlePoolSettings particlePoolSettings;
        public SquarePoolSettings squarePoolSettings;
        public SystemSettings sysSettings;
        public MissionSetting currentMissionSetting;
        public Dictionary<ItemAbilityType, ItemEntity> itemTable = new ();
        public Dictionary<AudiosEnum, AudioClip> audioClipDic = new ();
        public GameObject audioSourcePrefab;
        
        // 加载进度相关
        public float CurrentProgress { get; private set; } = 0f;
        private const float TOTAL_PROGRESS = 0.9f;
        private List<AssetLoadInfo> assetLoadInfos = new ();
        private float totalWeight = 0f;
        

        public AssetModule(GameContext ctx)
        {
            this.ctx = ctx;
            // 重置进度
            CurrentProgress = 0f;
            assetLoadInfos.Clear();
            totalWeight = 0f;
            
            // 注册所有要加载的资源
            RegisterAssetLoadInfo("Audio", 1f);
            RegisterAssetLoadInfo("Square", 1f);
            RegisterAssetLoadInfo("Shape", 1f);
            RegisterAssetLoadInfo("GridSo", 1f);
            // RegisterAssetLoadInfo("DestroyParticle", 1f);
            // RegisterAssetLoadInfo("ParticlePoolSettings", 1f);
            RegisterAssetLoadInfo("SquarePoolSettings", 1f);
            RegisterAssetLoadInfo("PGCSettings", 1f);
            RegisterAssetLoadInfo("ItemTable", 1f);
            RegisterAssetLoadInfo("Popup", 1f);
            RegisterAssetLoadInfo("ShapeImage", 1f);
            RegisterAssetLoadInfo("AudioSourcePrefab", 1f);
            // 计算总权重和每个资源的实际权重
            CalculateWeights();
        }
        
        public IEnumerator LoadAllAssets()
        {
            yield return LoadAssets<AudioClip>("Audio", (audioList) =>
            {
                foreach (var audio in audioList)
                {
                    if (global::System.Enum.TryParse(audio.name, out AudiosEnum audioEnum))
                    {
                        audioClipDic.Add(audioEnum, audio);
                    }
                }
            });
            
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
            
            yield return LoadAsset("ParticlePoolSettings", (ParticlePoolSettings settings) =>
            {
                particlePoolSettings = settings;
            });
            
            yield return LoadAsset<SquarePoolSettings>("SquarePoolSettings", (settings) =>
            {
                squarePoolSettings = settings;
            });
            
            yield return LoadAsset<SystemSettings>("PGCSettings", (settings) =>
            {
                sysSettings = settings;
                currentMissionSetting = new MissionSetting();
                currentMissionSetting.missionID = 1;
                currentMissionSetting.moveSystemMoveInterval = sysSettings.moveSystemMoveInterval;
                currentMissionSetting.moveSystemAutoMoveInterval = sysSettings.moveSystemAutoMoveInterval;
                currentMissionSetting.horizontalMoveDelay = sysSettings.horizontalMoveDelay;
                currentMissionSetting.specialItemHitRate = sysSettings.specialItemHitRate;
                currentMissionSetting.enableSpecialItems = true;
                currentMissionSetting.specialItemMode = SpecialItemMode.PositiveOnly;
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

            yield return LoadAsset<GameObject>("AudioSourcePrefab", (prefab) =>
            {
                audioSourcePrefab = prefab;
            });
            
            // 确保进度达到0.9
            CurrentProgress = TOTAL_PROGRESS;
        }
        
        // 注册资源加载信息
        private void RegisterAssetLoadInfo(string name, float baseWeight)
        {
            assetLoadInfos.Add(new AssetLoadInfo { Name = name, Weight = baseWeight });
        }
        
        // 计算权重
        private void CalculateWeights()
        {
            // 计算总权重
            totalWeight = 0f;
            foreach (var info in assetLoadInfos)
            {
                totalWeight += info.Weight;
            }
            
            // 计算每个资源的实际权重（相对于总进度0.9）
            foreach (var info in assetLoadInfos)
            {
                info.Weight = (info.Weight / totalWeight) * TOTAL_PROGRESS;
            }
        }
        
        // 更新进度
        private void UpdateProgress(string assetName)
        {
            var info = assetLoadInfos.Find(i => i.Name == assetName);
            if (info != null && !info.IsLoaded)
            {
                info.IsLoaded = true;
                CurrentProgress += info.Weight;
                ctx.loadingUI.value = CurrentProgress;
                // Debug.Log($"Asset {assetName} loaded. Progress: {CurrentProgress:F2}/{TOTAL_PROGRESS}");
            }
        }
        
        public IEnumerator LoadAssets<T>(string label, Action<IList<T>> callback)
        {
            var handle =  Addressables.LoadAssetsAsync<T>(label, null);
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"LoadAssets<{label}> failed: {handle.Status}");
                // 即使加载失败也要更新进度，避免卡在某个资源上
                UpdateProgress(label);
                yield break;
            }
            callback?.Invoke(handle.Result);
            UpdateProgress(label);
        }
        
        public IEnumerator LoadAsset<T>(string key, Action<T> callback)
        {
            var handle =  Addressables.LoadAssetAsync<T>(key);
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"LoadAsset<{key}> failed: {handle.Status}");
                // 即使加载失败也要更新进度，避免卡在某个资源上
                UpdateProgress(key);
                yield break;
            }
            callback?.Invoke(handle.Result);
            UpdateProgress(key);
        }

        public bool ShapeSoTryGetRandom(out SquareShapeSo squareShapeSo)
        {
            if (squareShapeSoList == null || squareShapeSoList.Count == 0)
            {
                squareShapeSo = null;
                return false;
            }
            // 随机7-bag
            if (shapeSoBagQueue.Count <= 0)
            {
                ConstantTool.Shuffle(squareShapeSoList);
                foreach (var shapeSo in squareShapeSoList)
                {
                    shapeSoBagQueue.Enqueue(shapeSo);
                }
            }
            squareShapeSo = shapeSoBagQueue.Dequeue();
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