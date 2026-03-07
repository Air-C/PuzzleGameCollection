using System;
using PGC.Controller;

namespace PGC.System_Game {

    public static class GameSystem {

        public static void NewGame(GameContext ctx) {
            ctx.state_game.isRunning = true;
            MissionController.NewGame(ctx);
        }

        public static void Tick(GameContext ctx, float dt) {
            if (!ctx.state_game.isRunning) {
                return;
            }

            // 1. Process Input
            ctx.inputModule.Tick(dt);

            // 2. Do Logic
            ctx.restTime += dt;
            float fixInterval = GameConst.fixInterval;
            do {
                float step = MathF.Min(ctx.restTime, fixInterval);
                ctx.restTime -= step;
                FixTick(ctx, step);
            } while (ctx.restTime >= fixInterval);

            // 3. Render
            MissionController.Render(ctx);

            // if pause
            // ctx.state_game.isRunning = false;
            // ctx.events_game.OnPauseInvoke();
        }

        static void FixTick(GameContext ctx, float fixdt) {
            MissionController.FixTick(ctx, fixdt);
        }

    }

}