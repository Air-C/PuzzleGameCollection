using System;
using System.Collections;
using UnityEngine;

namespace PGC.Controller {

    // 生成, 销毁, Tick
    public static class SquareController {

        public static SquareEntity Spawn(GameContext ctx, int typeID) {
            bool has = ctx.assetModule.Square_TryGet(typeID, out SquareSO squareSO);
            if (!has) {
                Debug.LogError($"Failed to spawn SquareEntity with typeID {typeID} because SquareSO not found");
                return null;
            }
            return SpawnBySO(ctx, squareSO);
        }

        public static SquareEntity SpawnBySO(GameContext ctx, SquareSO so) {
            var squarePrefab = so.prefab;
            if (squarePrefab == null) {
                Debug.LogError("Failed to spawn SquareEntity because prefab is not loaded");
                return null;
            }

            // Create
            var entity = GameObject.Instantiate(squarePrefab);
            entity.id = ctx.userEntity.ID_Square();

            // Add
            ctx.squareRepository.Add(entity);
            return entity;
        }

        public static void Unspawn(GameContext ctx, SquareEntity entity) {
            // Remove
            ctx.squareRepository.Remove(entity);

            // Destroy
            GameObject.Destroy(entity.gameObject);
        }

        // 高层的销毁
        public static void Die(GameContext ctx, SquareEntity entity) {
            // 播特效, 音效, 手柄震动
            Unspawn(ctx, entity);
        }

        // 旋转, 移动, 消除
        public static void MoveDown(GameContext ctx, SquareEntity entity) {
            entity.GridIndex += Int2.Down;
            entity.TF_Pos_Set(GridHelper.GetSquareWorldPos(entity.GridIndex));
        }

    }
}