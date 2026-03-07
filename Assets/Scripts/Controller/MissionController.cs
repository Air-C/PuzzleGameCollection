// 一关
using System;
using UnityEngine;

namespace PGC.Controller {

    public static class MissionController {

        public static void NewGame(GameContext ctx) {
            // 生成第一Shape
            ShapeEntity shape = Shape_Gen(ctx);

            // 准备第二Shape
            var mission = ctx.missionEntity;
            ctx.assetModule.Shape_TryGetRandom(out var nextShapeSO);
            mission.shapeType_next = nextShapeSO.shapeType;

            mission.downTimer = 0;
            mission.downInterval = 0.5f;
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

        public static void FixTick(GameContext ctx, float fixdt) {

            var mission = ctx.missionEntity;
            var shape = ctx.shapeEntity;

            // 自动下落
            mission.downTimer -= fixdt;
            if (mission.downTimer <= 0) {
                mission.downTimer += mission.downInterval; // 注意: 往后所有CD都是这样写

                if (shape != null) {
                    ShapeController.MoveDown(ctx, shape);
                }
            }

            // 左右移动
            var input = ctx.inputModule;
            if (input.moveHorizontalInput.status == InputStatus.Pressed || input.moveHorizontalInput.status == InputStatus.Held) {
                if (shape != null) {
                    ShapeController.MoveHorizontal(ctx, shape, (int)input.moveHorizontalInput.value);
                    Debug.Log($"MoveHorizontal with value {input.moveHorizontalInput.value}");
                }
            }
        }

        public static void Render(GameContext ctx) {

        }

    }

}