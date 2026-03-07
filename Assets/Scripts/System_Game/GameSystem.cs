using System;
using PGC.Controller;

namespace PGC.System_Game {

    public static class GameSystem {

        public static void NewGame(GameContext ctx) {
            MissionController.NewGame(ctx);
        }

        public static void Tick(GameContext ctx, float dt) {
            if (!ctx.state_game.isRunning) {
                return;
            }

            // 1. Process Input
            ctx.inputModule.Tick(dt);

            // 2. Do Logic

            // 3. Render
            MissionController.Tick(ctx);

            // if pause
            // ctx.state_game.isRunning = false;
            // ctx.events_game.OnPauseInvoke();
        }

    }

}