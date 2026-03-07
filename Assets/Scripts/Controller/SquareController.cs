using System;
using System.Collections;
using UnityEngine;

namespace PGC.Controller {

    // 生成, 销毁, Tick
    public static class SquareController {

        public static SquareEntity Spawn(GameContext ctx, ShapeType type) {
            // Create
            var entity = new SquareEntity();
            entity.id = ctx.userEntity.ID_Square();

            // TODO: 

            // Add
            ctx.squareRepository.Add(entity);
            return entity;
        }

        public static void Unspawn(GameContext ctx, SquareEntity entity) {
            // Remove
            ctx.squareRepository.Remove(entity);

            // Destroy
        }

        // 高层的销毁
        public static void Die(GameContext ctx, SquareEntity entity) {
            // 播特效, 音效, 手柄震动
            Unspawn(ctx, entity);
        }

        // 旋转, 移动, 消除

    }
}