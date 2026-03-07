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
            foreach (var gridIndex in shapeSO.squareIndices) {
                var squareEntity = SquareController.Spawn(ctx);
                if (squareEntity == null) {
                    Debug.LogError($"Failed to spawn SquareEntity for ShapeEntity with shapeType {type}");
                    continue;
                }
                squareEntity.GridIndex = startGridIndex + gridIndex;
                entity.AddSquare(squareEntity.id);
            }

            ctx.shapeEntity = entity;

            return entity;
        }

        public static void Unspawn(GameContext ctx, ShapeEntity entity) {
            ctx.shapeEntity = null;
        }

    }

}