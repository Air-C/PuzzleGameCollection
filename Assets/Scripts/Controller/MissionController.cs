// 一关
using System;
using UnityEngine;

namespace PGC.Controller {

    public static class MissionController {

        public static void NewGame(GameContext ctx) {
            // 生成第一Shape
            ShapeEntity shape = Shape_Gen(ctx);

            // 准备第二Shape
            ctx.assetModule.Shape_TryGetRandom(out var nextShapeSO);
            ctx.missionEntity.shapeType_next = nextShapeSO.shapeType;
        }

        static ShapeEntity Shape_Gen(GameContext ctx) {
            // 生成一个随机Shape, 放在上方预备区
            bool has = ctx.assetModule.Shape_TryGetRandom(out var shapeSO);
            if (!has) {
                Debug.LogError($"Failed to generate ShapeEntity because ShapeSO not found");
                return null;
            }

            Int2 topCenter = GameConst.gridTopCenter;
            return ShapeController.Spawn(ctx, shapeSO.shapeType, topCenter);
        }

        public static void Tick(GameContext ctx) {

        }

    }

}