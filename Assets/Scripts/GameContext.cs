using System;

namespace PGC {

    // 存储游戏里的所有数据, 以及模块入口
    public class GameContext {

        // ==== System ====
        public GameSystemEvents events_game;
        public GameSystemState state_game;

        // ==== Module ====
        public AssetModule assetModule;

        // ==== Entity ====
        public UserEntity userEntity;
        public ShapeEntity shapeEntity;
        public MissionEntity missionEntity;
        public SquareRepository squareRepository;

        public GameContext() { }

    }
}