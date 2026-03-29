using System.Collections.Generic;
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
        private List<SquarePoolModel> specialPool = new();
        private List<SquarePoolModel> normalPool = new();
        
        
        public SquarePool(GameContext ctx)
        {
            this.ctx = ctx;
            InitSquarePool();
        }

        void InitSquarePool()
        {
            poolManager = Object.Instantiate(ctx.assetModule.squarePoolSettings.poolManager);
            foreach (var squarePoolAssetModel in ctx.assetModule.squarePoolSettings.poolSettings)
            {
                SquarePoolModel squarePoolModel = new ();
                squarePoolModel.squareName = squarePoolAssetModel.prefab.name;
                EnqueueSquarePool(squarePoolModel, squarePoolAssetModel);
                if (squarePoolAssetModel.abilityType == ItemAbilityType.None)
                {
                    normalPool.Add(squarePoolModel);
                }
                else
                {
                    specialPool.Add(squarePoolModel);
                }
            }
        }

        void ExtendSquarePool(SquarePoolModel queue)
        {
            SquarePoolAssetModel squarePoolAssetModel = null;
            foreach (var setting in ctx.assetModule.squarePoolSettings.poolSettings)
            {
                if (queue.squareName == setting.prefab.name)
                {
                    squarePoolAssetModel = setting;
                    break;
                }
            }
            if (squarePoolAssetModel == null)
            {
                return;
            }
            EnqueueSquarePool(queue, squarePoolAssetModel);
        }

        void EnqueueSquarePool(SquarePoolModel queue,SquarePoolAssetModel squarePoolAssetModel)
        {
            for (int i = 0; i < squarePoolAssetModel.size; i++)
            {
                SquareEntity square = new SquareEntity(-1, -1);
                square.SquareObj = Object.Instantiate(squarePoolAssetModel.prefab, poolManager.transform);
                square.AbilityType = squarePoolAssetModel.abilityType;
                square.SquareName = squarePoolAssetModel.prefab.name;
                queue.squarePool.Enqueue(square);
            }
        }

        public List<SquareEntity> GetSquareByIndexes(List<Vector2Int> indexes)
        {
            if (ConstantTool.Hit(ctx.assetModule.sysSettings.specialItemHitRate))
            {
                return GetSpecialSquareByShape(indexes);
            }
            else
            {
                return GetNormalSquareByShape(indexes);
            }
        }

        public List<SquareEntity> GetNormalSquareByShape(List<Vector2Int> indexes)
        {
            List<SquareEntity> list = new List<SquareEntity>();
            int poolIndex = Random.Range(0, normalPool.Count);
            SquarePoolModel currentQueue = normalPool[poolIndex];
            SquareQueueInventoryCheck(ref currentQueue, indexes.Count);
            foreach (var index in indexes)
            {
                SquareEntity squareEntity = currentQueue.squarePool.Dequeue();
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
            SquarePoolModel currentNormalQueue = normalPool[Random.Range(0, normalPool.Count)];
            SquareQueueInventoryCheck(ref currentNormalQueue, indexes.Count);
            SquarePoolModel currentSpecialQueue = specialPool[Random.Range(0, specialPool.Count)];
            SquareQueueInventoryCheck(ref currentNormalQueue, 1);
            int rand = Random.Range(0, indexes.Count);
            int count = 0;
            foreach (var index in indexes)
            {
                 
                SquareEntity squareEntity = count == rand ? currentSpecialQueue.squarePool.Dequeue() : currentNormalQueue.squarePool.Dequeue();
                squareEntity.RestIndex(index.x, index.y);
                squareEntity.SquareObj.transform.position = ctx.grid.GetWorldPositionByIndex(index);
                squareEntity.SquareObj.SetActive(true);
                list.Add(squareEntity);
                count++;
            }
            return list;
        }
        
        

        void SquareQueueInventoryCheck(ref SquarePoolModel currentQueue, int atLestCount = 4)
        {
            if (currentQueue.squarePool.Count < atLestCount)
            {
                foreach (var normalQueue in normalPool)
                {
                    if (normalQueue.squarePool.Count >= atLestCount)
                    {
                        currentQueue = normalQueue;
                        break;
                    }
                }
            }
            if (currentQueue.squarePool.Count < atLestCount)
            {
                ExtendSquarePool(currentQueue);
            }
        }
        
        public void ReturnSquareByIndexes()
        {
            if (ctx.squaresWaitForDestroy.squares.Count == 0)
            {
                return;
            }
            Dictionary<string, List<SquareEntity>> waitForDecSqDic = new Dictionary<string, List<SquareEntity>>();
            foreach (var square in ctx.squaresWaitForDestroy.squares)
            {
                if (waitForDecSqDic.TryGetValue(square.SquareName, out List<SquareEntity> list))
                {
                    list.Add(square);
                }
                else
                {
                    waitForDecSqDic.Add(square.SquareName, new List<SquareEntity>()
                    {
                        square
                    });
                }
            }
            DeactivateSquarePool(waitForDecSqDic, specialPool);
            DeactivateSquarePool(waitForDecSqDic, normalPool);
            ctx.squaresWaitForDestroy.squares.Clear();
        }

        void DeactivateSquarePool(Dictionary<string, List<SquareEntity>> squarePoolDic, List<SquarePoolModel> squarePoolModelList)
        {
            foreach (var squarePoolModel in squarePoolModelList)
            {
                if (squarePoolDic.TryGetValue(squarePoolModel.squareName, out List<SquareEntity> list))
                {
                    foreach (var squareEntity in list)
                    {
                        squarePoolModel.squarePool.Enqueue(squareEntity);
                        squareEntity.SquareObj.SetActive(false);
                    }
                }
            }
        }
        
    }
}