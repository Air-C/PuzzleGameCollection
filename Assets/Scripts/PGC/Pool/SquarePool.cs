using System.Collections.Generic;
using System.Linq;
using Custom.Tool;
using Entities;
using PGC.Enum;
using PGC.Pool.Model;
using PGC.System;
using Unity.Mathematics.Geometry;
using UnityEngine;

namespace PGC.Pool
{
    public class SquarePool
    {
        private GameContext ctx;
        GameObject poolManager;
        
        private readonly Dictionary<SquareColorEnum, Queue<SquareEntity>> positivePool = new();
        private readonly Dictionary<SquareColorEnum, Queue<SquareEntity>> negativePool = new();
        private readonly Dictionary<SquareColorEnum, Queue<SquareEntity>> normalPool = new();
        private readonly Queue<SquareEntity> punishmentPool = new();
        private readonly List<SquareColorEnum> positiveColors = new();
        private readonly List<SquareColorEnum> negativeColors = new();
        private readonly List<SquareColorEnum> normalColors = new();
        
        public SquarePool(GameContext ctx)
        {
            this.ctx = ctx;
            InitSquarePool();
        }

        void InitSquarePool()
        {
            poolManager = Object.Instantiate(ctx.assetModule.squarePoolSettings.poolManager);
            EnqueueSquarePool(punishmentPool, ctx.assetModule.squarePoolSettings.punishmentSetting);
            foreach (var squarePoolAssetModel in ctx.assetModule.squarePoolSettings.poolSettings)
            {
                Queue<SquareEntity> squarePoolQueue = new();
                EnqueueSquarePool(squarePoolQueue, squarePoolAssetModel);
                if (squarePoolAssetModel.abilityType == ItemAbilityType.None)
                {
                    normalPool.Add(squarePoolAssetModel.squareColor, squarePoolQueue);
                    normalColors.Add(squarePoolAssetModel.squareColor);
                }
                else if (IsNegativeAbility(squarePoolAssetModel.abilityType))
                {
                    negativePool.Add(squarePoolAssetModel.squareColor, squarePoolQueue);
                    negativeColors.Add(squarePoolAssetModel.squareColor);
                }
                else
                {
                    positivePool.Add(squarePoolAssetModel.squareColor, squarePoolQueue);
                    positiveColors.Add(squarePoolAssetModel.squareColor);
                }
            }
        }

        void ExtendSquarePool(SquareColorEnum key, Dictionary<SquareColorEnum, Queue<SquareEntity>> dic)
        {
            SquarePoolAssetModel squarePoolAssetModel = null;
            foreach (var setting in ctx.assetModule.squarePoolSettings.poolSettings)
            {
                if (key == setting.squareColor)
                {
                    squarePoolAssetModel = setting;
                    break;
                }
            }
            if (squarePoolAssetModel == null)
            {
                return;
            }
            if (dic.TryGetValue(key, out Queue<SquareEntity> queue))
            {
                EnqueueSquarePool(queue, squarePoolAssetModel);
            }
        }

        void EnqueueSquarePool(Queue<SquareEntity> queue, SquarePoolAssetModel squarePoolAssetModel)
        {
            for (int i = 0; i < squarePoolAssetModel.size; i++)
            {
                SquareEntity square = new SquareEntity(-1, -1);
                square.SquareObj = Object.Instantiate(squarePoolAssetModel.prefab, poolManager.transform);
                square.AbilityType = squarePoolAssetModel.abilityType;
                square.SquareColor = squarePoolAssetModel.squareColor;
                queue.Enqueue(square);
            }

        }

        public List<SquareEntity> GetSquareByIndexes(List<Vector2Int> indexes, bool isPunishment = false)
        {
            if (isPunishment)
            {
                return GetPunishmentSquare(indexes);
            }

            var missionSetting = ctx.assetModule.currentMissionSetting;
            if (!missionSetting.enableSpecialItems || missionSetting.specialItemMode == SpecialItemMode.Disabled)
            {
                return GetNormalSquareByShape(indexes);
            }

            if (ConstantTool.Hit(missionSetting.specialItemHitRate))
            {
                List<SquareColorEnum> availableColors = GetAvailableSpecialColors(missionSetting.specialItemMode);
                if (availableColors.Count > 0)
                {
                    return GetSpecialSquareByShape(indexes, availableColors);
                }
            }

            return GetNormalSquareByShape(indexes);
        }

        List<SquareColorEnum> GetAvailableSpecialColors(SpecialItemMode mode)
        {
            List<SquareColorEnum> available = new List<SquareColorEnum>();
            if (mode == SpecialItemMode.PositiveOnly || mode == SpecialItemMode.Both)
            {
                available.AddRange(positiveColors);
            }
            if (mode == SpecialItemMode.NegativeOnly || mode == SpecialItemMode.Both)
            {
                available.AddRange(negativeColors);
            }
            return available;
        }

        static bool IsNegativeAbility(ItemAbilityType abilityType)
        {
            return abilityType == ItemAbilityType.AddRow1;
        }

        Dictionary<SquareColorEnum, Queue<SquareEntity>> GetSpecialPoolDict(SquareColorEnum color)
        {
            if (positivePool.ContainsKey(color))
            {
                return positivePool;
            }
            return negativePool;
        }
        
        public List<SquareEntity> GetPunishmentSquare(List<Vector2Int> indexes)
        {
            List<SquareEntity> list = new List<SquareEntity>();
            foreach (var index in indexes)
            {
                SquareEntity squareEntity = punishmentPool.Dequeue();
                squareEntity.RestIndex(index.x, index.y);
                squareEntity.SquareObj.transform.position = ctx.grid.GetWorldPositionByIndex(index);
                squareEntity.SquareObj.SetActive(true);
                list.Add(squareEntity);
            }
            return list;
        }

        public List<SquareEntity> GetNormalSquareByShape(List<Vector2Int> indexes)
        {
            List<SquareEntity> list = new List<SquareEntity>();
            int poolIndex = Random.Range(0, normalColors.Count);
            var randomKey = normalColors[poolIndex];
            randomKey = SquareQueueInventoryCheck(randomKey, normalPool, indexes.Count);
            Queue<SquareEntity> currentQueue = normalPool[randomKey];
            foreach (var index in indexes)
            {
                SquareEntity squareEntity = currentQueue.Dequeue();
                squareEntity.RestIndex(index.x, index.y);
                squareEntity.SquareObj.transform.position = ctx.grid.GetWorldPositionByIndex(index);
                squareEntity.SquareObj.SetActive(true);
                list.Add(squareEntity);
            }
            return list;
        }
        
        public List<SquareEntity> GetSpecialSquareByShape(List<Vector2Int> indexes, List<SquareColorEnum> availableColors)
        {
            List<SquareEntity> list = new List<SquareEntity>();
            var randomNormalKey = normalColors[Random.Range(0, normalColors.Count)];
            randomNormalKey = SquareQueueInventoryCheck(randomNormalKey, normalPool, indexes.Count-1);
            Queue<SquareEntity> currentNormalQueue = normalPool[randomNormalKey];

            var randomSpecialKey = availableColors[Random.Range(0, availableColors.Count)];
            var specialPoolDic = GetSpecialPoolDict(randomSpecialKey);
            randomSpecialKey = SquareQueueInventoryCheck(randomSpecialKey, specialPoolDic, 1);
            Queue<SquareEntity> currentSpecialQueue = specialPoolDic[randomSpecialKey];
            
            int rand = Random.Range(0, indexes.Count);
            int count = 0;
            foreach (var index in indexes)
            {
                SquareEntity squareEntity = count == rand ? currentSpecialQueue.Dequeue() : currentNormalQueue.Dequeue();
                squareEntity.RestIndex(index.x, index.y);
                squareEntity.SquareObj.transform.position = ctx.grid.GetWorldPositionByIndex(index);
                squareEntity.SquareObj.SetActive(true);
                list.Add(squareEntity);
                count++;
            }
            return list;
        }
        
        

        SquareColorEnum SquareQueueInventoryCheck(SquareColorEnum randomKey, Dictionary<SquareColorEnum, Queue<SquareEntity>> dic, int atLestCount = 4)
        {
            Queue<SquareEntity> currentQueue = dic[randomKey];
            if (currentQueue.Count < atLestCount)
            {
                foreach (var keyValuePair in dic)
                {
                    if (keyValuePair.Value.Count >= atLestCount)
                    {
                        return keyValuePair.Key;
                    }
                }
                ExtendSquarePool(randomKey, dic);
            }
            return randomKey;
        }
        
        public void ReturnSquareByIndexes()
        {
            if (ctx.squaresWaitForDestroy.squares.Count == 0)
            {
                return;
            }
            Dictionary<SquareColorEnum, Queue<SquareEntity>> waitForDecSqDic = new ();
            foreach (var square in ctx.squaresWaitForDestroy.squares)
            {
                if (waitForDecSqDic.TryGetValue(square.SquareColor, out Queue<SquareEntity> queue))
                {
                    queue.Enqueue(square);
                }
                else
                {
                    waitForDecSqDic.Add(square.SquareColor, new Queue<SquareEntity>(new[] {square}));
                }
            }
            DeactivateSquarePool(waitForDecSqDic, positivePool);
            DeactivateSquarePool(waitForDecSqDic, negativePool);
            DeactivateSquarePool(waitForDecSqDic, normalPool);
            if (waitForDecSqDic.TryGetValue(ctx.assetModule.squarePoolSettings.punishmentSetting.squareColor,
                    out Queue<SquareEntity> punishmentQueue))
            {
                while (punishmentQueue.Count > 0)
                {
                    SquareEntity squareEntity = punishmentQueue.Dequeue();
                    squareEntity.SquareObj.SetActive(false);
                    punishmentPool.Enqueue(squareEntity);
                }
            }
            ctx.squaresWaitForDestroy.squares.Clear();
        }

        void DeactivateSquarePool(Dictionary<SquareColorEnum, Queue<SquareEntity>> waitForDecSqDic, Dictionary<SquareColorEnum, Queue<SquareEntity>> squarePoolDic)
        {
            foreach (var keyValuePair in waitForDecSqDic)
            {
                if (squarePoolDic.TryGetValue(keyValuePair.Key, out Queue<SquareEntity> queue))
                {
                    while (keyValuePair.Value.Count > 0)
                    {
                        SquareEntity squareEntity = keyValuePair.Value.Dequeue();
                        squareEntity.SquareObj.SetActive(false);
                        queue.Enqueue(squareEntity);
                    }
                }
            }
        }
        
    }
}