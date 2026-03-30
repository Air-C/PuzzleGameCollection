using System.Collections.Generic;
using System.Linq;
using Custom.Tool;
using Entities;
using PGC.Enum;
using PGC.Pool.Model;
using Unity.Mathematics.Geometry;
using UnityEngine;

namespace PGC.Pool
{
    public class SquarePool
    {
        private GameContext ctx;
        GameObject poolManager;
        
        private readonly Dictionary<SquareColorEnum, Queue<SquareEntity>> specialPool = new();
        private readonly Dictionary<SquareColorEnum, Queue<SquareEntity>> normalPool = new();
        private readonly Queue<SquareEntity> punishmentPool = new();
        private readonly List<SquareColorEnum> specialColors = new();
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
                else
                {
                    specialPool.Add(squarePoolAssetModel.squareColor, squarePoolQueue);
                    specialColors.Add(squarePoolAssetModel.squareColor);
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
            
            if (ConstantTool.Hit(ctx.assetModule.sysSettings.specialItemHitRate))
            {
                return GetSpecialSquareByShape(indexes);
            }
            else
            {
                return GetNormalSquareByShape(indexes);
            }
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
        
        public List<SquareEntity> GetSpecialSquareByShape(List<Vector2Int> indexes)
        {
            List<SquareEntity> list = new List<SquareEntity>();
            var randomNormalKey = normalColors[Random.Range(0, normalColors.Count)];
            randomNormalKey = SquareQueueInventoryCheck(randomNormalKey, normalPool, indexes.Count-1);
            Queue<SquareEntity> currentNormalQueue = normalPool[randomNormalKey];

            var randomSpecialKey = specialColors[Random.Range(0, specialColors.Count)];
            randomSpecialKey = SquareQueueInventoryCheck(randomSpecialKey, specialPool, 1);
            Queue<SquareEntity> currentSpecialQueue = specialPool[randomSpecialKey];
            
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
            DeactivateSquarePool(waitForDecSqDic, specialPool);
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