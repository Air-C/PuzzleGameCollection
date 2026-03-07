using System;
using UnityEngine;

namespace PGC.Controller {

    public static class ShapeController {

        public static ShapeEntity Spawn(GameContext ctx, ShapeType type, Int2 startGridIndex) {
            bool has = ctx.assetModule.Shape_TryGet(type, out ShapeSO shapeSO);
            if (!has) {
                Debug.LogError($"Failed to spawn ShapeEntity with shapeType {type} because ShapeSO not found");
                return null;
            }

            // Create
            var entity = new ShapeEntity();
            bool hasSquare = ctx.assetModule.Square_TryGetRandom(out var squareSO);
            foreach (var gridIndex in shapeSO.squareIndices) {
                var squareEntity = SquareController.SpawnBySO(ctx, squareSO);
                if (squareEntity == null) {
                    Debug.LogError($"Failed to spawn SquareEntity for ShapeEntity with shapeType {type}");
                    continue;
                }
                squareEntity.GridIndex = startGridIndex + gridIndex;
                squareEntity.TF_Pos_Set(GridHelper.GetSquareWorldPos(squareEntity.GridIndex));
                entity.AddSquare(squareEntity.id);
            }

            ctx.shapeEntity = entity;

            return entity;
        }

        public static void Unspawn(GameContext ctx, ShapeEntity entity) {
            ctx.shapeEntity = null;
        }

        public static void MoveDown(GameContext ctx, ShapeEntity entity) {
            foreach (var squareId in entity.squareIds) {
                bool has = ctx.squareRepository.TryGet(squareId, out var squareEntity);
                if (!has) {
                    Debug.LogError($"Failed to MoveDown ShapeEntity because SquareEntity with id {squareId} not found");
                    continue;
                }

                SquareController.MoveDown(ctx, squareEntity);
            }
        }

        public static void MoveHorizontal(GameContext ctx, ShapeEntity entity, int dir) {
            foreach (var squareId in entity.squareIds) {
                bool has = ctx.squareRepository.TryGet(squareId, out var squareEntity);
                if (!has) {
                    Debug.LogError($"Failed to MoveHorizontal ShapeEntity because SquareEntity with id {squareId} not found");
                    continue;
                }

                Int2 offset = new Int2(dir, 0);
                squareEntity.GridIndex += offset;
                squareEntity.TF_Pos_Set(GridHelper.GetSquareWorldPos(squareEntity.GridIndex));
            }
        }

    }

}